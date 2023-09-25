
import java.io.FileInputStream;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.util.Properties;
import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.apache.log4j.PropertyConfigurator;
/**
 * Activation Sample class.<br/>
 * This class handles the  only the UI part, check out the Activation class<br/>
 * which contains all the business logic for activation.<p/>
 * 
 * The sample reads its init parameters from the activation.properties class.<br/>
 * you can modify the entries in the properties file if needed.
 * 
 * 
 *
 */
public class ActivationSample {
	/**input data properties keys*/
	public static final String PROP_KEY_URL = "url";
	public static final String PROP_KEY_PRODUCT_KEY = "product_key";
	
	public static final String PROP_KEY_FIRST_NAME = "first_name";
	public static final String PROP_KEY_LAST_NAME = "last_name";
	public static final String PROP_KEY_EMAIL = "email";
	
	/**registration indication flags values*/
	public static final int REG_NOTREQUIRED=1;
	public static final int REG_DESIRED=2;
	public static final int REG_MANDATORY=3;
	
	public static final String VENDOR_CODE = "KV5Aw3Efx74GZJvZTiRFfbYWAm2Zjk5oElgKTRt/jfRUhesQLq+eyNTTloPG9b+hAQwGZQoETGPPemq0h81eu0AT/Rfu+Yb1uhDyDCJ7R0NQH7sESpzm77wvvphJlmURpVYRiJ3YWGKJo4Yy9T46GBVbDdoyAesp9JiQZSfrfJgBAVekTJsr1nuBnTOoa9azbgLx/ZXKaN3T/9H+CO8NQ/Ii8ZYtPUPk1S0AXfkNfyJ8BB7XAyRa/+d2B6gLKqPIaS7g69QBjAQH7r9LQP/vujDiyTgieDdWaOQtey5uMMLF+9IUVxTardenkkR4vfZOE4dp8mFrS48dfpKW5G4dWDHdxhYt3ETz7rKeORUX8glukb2HRIp/csRfWYcAa8XzuH2qsr9bRTDJVPl1hMTR3x8Iya2blivLZkBjVPfhcKz4emdLO3zjg6FMHoKJGadtttNy5fp3P6uozupNTQI8ihyRAE9I5k9Mf39N5/lWctjRXNB9sVouBowS1Z5fNzSqhPKSwybpNB65O3LM7me3uQKxk0uHySgE/fRc+TrPypJ2YJ66Z9fPWdFlGY7Uj+omCpguTOJCh8rXYJ+MAzvSUilTbcLo6sVcuFeXF4dVmZ3lHZfwHOfCfqlkGBV7A1Xyh+FdDpbXvBREoU8oUdcW9vFbVtoscp5He+l1rZVgstZzsE8Br2RzTiaJsG0eHzl6zW4AjMNwOR0vVIel9SFo25/zShu1jC02rjJJBNoTIz3k5tfW+XKIjJQmXGhnMU1TNVMqBKaQAprnkZWMBBqCgDSlHH8KLZqAe3vUx/UWWbndXuHlbo9Oo5hQEqA/NUQ6RI6EwJQUstVgDdRtKi3+eNnkMl/JbrzVpe0EdV9ZnKQPT1C8A9kjDHMMgwivzr38mzfyJQMiMM6svt3RTbJ9wvy3ZtoYu+Bfy1LF5MEVODE3PlIJkgSyHFOOaiBkLMx8";
	
	protected final Log log= LogFactory.getLog(this.getClass());
	
	
	/**input data*/
	protected Properties props = null;
	
	/**the jsessionid for initiating WS calls to the EMS server*/
	protected String jSessionId = null;
	
	/**flag indicating if registration is needed*/
	protected int registrationStat = 0;
	protected String redirectToUserReg = "";

	
	protected String createWelcomeMessage(){
		StringBuffer sb = new StringBuffer();
		sb.append("\nWelcome to the Activation Sample!\n");
		sb.append("\n");
		sb.append("This sample demonstrates the steps required to activate an SL license and\n register an end user.\n");
		sb.append("1. You first use the \"hasp_get_info\" function to retrieve a fingerprint \nof the end user's computer. \n");
		sb.append("2. The fingerprint is used together with a Product Key in Sentinel EMS to \ngenerate an Activation V2C file.\n");
		sb.append("3. Next, you apply the V2C file to the end user's computer using the \n\"hasp_update\" function.\n");
		sb.append("\n");
		sb.append("Note:\n" );
		sb.append("Sentinel EMS URL, Entitlement Product Key and Registration info are \nconfigurable in the activation.properties file.\n");
		sb.append("\n");
		sb.append("Press <Enter> to continue.");
		return sb.toString();
	}
	
	/**
	 * 
	 * @return
	 */
	protected boolean checkForEnter(){
		InputStream in = null;
		try{
			byte[] b = new byte[10];
			in = System.in;
			int i = in.read(b);
			char c = (char)b[0];
			
			if(c== '\n' || c=='\r'){
				return true;
			}
		}catch(Exception e){
			log.error("Exception thrown in checkForEnter "+e.getMessage(),e);
		}
		return false;
	}
	
	/**
	 * 
	 */
	public void activate(){
		System.out.println(createWelcomeMessage());
		boolean continueActivation  = checkForEnter(); 
		if(!continueActivation){
			return;
		}
		props = new Properties();
		try{
			loadProps();
			if(customerLogin()){
				log.info("Registering customer...");
				//check if registration is required
				if(registrationStat == REG_MANDATORY || registrationStat == REG_DESIRED){
					if(redirectToUserReg.equalsIgnoreCase("true")){
						register();
					}else{
						System.out.println("Registration has already been performed for this Product Key.");
					}
				}
				log.info("activating...");
				doActivation();
			}
		}catch(Exception e){
			log.error("Error activating: "+e.getMessage(),e);
			System.out.println(e.getMessage());
		}
	}
	
	
	
	/**
	 * Registers the customer 
	 * 
	 * @return true if registration succeeds
	 */
	protected boolean register(){
		System.out.println("Registering...");
		String firstName = props.getProperty(PROP_KEY_FIRST_NAME);
		String lastName = props.getProperty(PROP_KEY_LAST_NAME);
		String email = props.getProperty(PROP_KEY_EMAIL);
		String url = props.getProperty(PROP_KEY_URL);
		//validate input params
		if(firstName==null || lastName==null || email==null || firstName.equals("") || lastName.equals("") || email.equals("")){
			System.out.println("Some of the customer registration information was not set.");
			System.out.println("Check the activation.properties file.");
			log.info("Some of the customer registration information was not set. Check the activation.properties file.");
			return false;
		}
		//create a customer object with mandatory fields
		Customer customer = new Customer(firstName,lastName,email);
		//register
		try{
			int response = Activation.register(url+"/",jSessionId, customer);
			if(response == Activation.CREATED){
				System.out.println("Customer registered successfully.");
				return true;
			}
		}catch(Exception e){
			log.error("Error : "+e.getMessage() , e );
			System.out.println("Error: "+e.getMessage());
		}
		return false;
	}
	
	
	/**
	 * This method does the following operation which are needed for activating:<br/>
	 * 1. Reads the C2V of the key.<br/>
	 * 2. Sends the C2V to the server and receives the license.</br>
	 * 3. Updates the key with the license.
	 * 
	 */
	protected void doActivation(){
		System.out.println("Activating...");
		//get url and product key code from the properties
		String url = props.getProperty(PROP_KEY_URL);
		String pk = props.getProperty(PROP_KEY_PRODUCT_KEY);
		pk = pk.trim();
		String c2v = readC2vFromKey(VENDOR_CODE);
		boolean ispassedServer = false;
		if(c2v!=null){
			try{
				String v2c = Activation.getLicense(url,jSessionId, c2v, pk);
				ispassedServer = true;
				if(v2c.contains("<hasp_info>")){
				Activation.updateKeyWithLicense(v2c,VENDOR_CODE);
				System.out.println("Key updated successfully.");
					log.info("Key updated successfully.");
				}
			}catch(Exception e){
				if(ispassedServer){
					log.error("Runtime Error : "+e.getMessage() , e );
					System.out.println("Runtime Error: "+e.getMessage());
				}else{
					log.error("EMS Error : "+e.getMessage() , e );
					System.out.println("EMS Error: "+e.getMessage());
				}
			}
		}
	}
	
	/**
	 * Reads and returns the key's C2V
	 * @return the keys C2V
	 */
	protected String readC2vFromKey(String vendorCode){
		try{
			String c2v = Activation.readC2vFromKey(vendorCode);
			if(c2v == null){
				System.out.println("Error reading Sentinel protection key.");
				log.error("Error reading Sentinel protection key." );
			}else{
				return c2v;
			}
		}catch(Exception e){
			log.error("Error readC2vFromKey : " , e );
			System.out.println(e.getMessage());
		}
		return null;
	}
	
	
	/**
	 * Loads the input parameters from the properties file
	 * @throws Exception
	 */
	protected void loadProps()throws Exception{
		System.out.println("Loading input parameters from properties file...");
		
		InputStreamReader reader = null;
		try{
			reader = new InputStreamReader(new FileInputStream("activation.properties"),"utf-8");
			props.load(reader);
			System.out.println("Input parameters loaded successfully.");
		}finally{
			try{
				if(reader != null)reader.close();
			}catch(Exception e){
				System.out.println(e);
				log.error("Error loading properties ",e );
			}
		}
	}
	
	/**
	 * Logs in with product key and sets the jsessionid and the registration required flag
	 * @return true if login succeeded
	 * @throws Exception
	 */
	protected boolean customerLogin()throws Exception{
		System.out.println("Logging in...");
		String url = props.getProperty(PROP_KEY_URL);
		String pk = props.getProperty(PROP_KEY_PRODUCT_KEY);
		//validate input params
		if(url==null || url.equals("") || pk==null || pk.equals("")){
			System.out.println("Product key and URL cannot be null. Check the activation.properties file.");
			log.info("Error Product key and URL cannot be null. Check the activation.properties file. " );
			return false;
		}
		pk = pk.trim();
		//logging in
		String[] loginRes = Activation.customerLogin(url, pk);
		//validate login response
		if(loginRes == null){
			System.out.println("Login failed. No login data returned from server.");
			log.error("Error Login failed. No login data returned from server." );
			return false;
		}
		//validate and set jsessionid and registration flag
		jSessionId = loginRes[0];
		if(jSessionId == null){
			System.out.println("Login failed. Session not created.");
			log.error("Error Login failed. Session not created" );
			return false;
		}
		
		if(loginRes[1]!=null){
			registrationStat = Integer.parseInt(loginRes[1]);
		}
		if(loginRes[2]!=null){
			redirectToUserReg = loginRes[2];
		}
		System.out.println("You were logged in successfully.");
		return true;
	}
	
	
	/**
	 * @param args
	 */
	public static void main(String[] args) {
		PropertyConfigurator.configure("log4j.properties");
		ActivationSample activationSample = new ActivationSample();
		activationSample.activate();
	}
	
	

}
