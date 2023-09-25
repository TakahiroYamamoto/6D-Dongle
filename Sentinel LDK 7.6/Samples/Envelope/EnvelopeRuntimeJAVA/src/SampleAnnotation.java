
public class SampleAnnotation {

	
	SampleAnnotation()
	{
		System.out.println("Inside Constructor of sample application");
	
	}
	
@EnvelopeMethodProtectionAttributes(Protect = true)
public void myAnnotationTestMethod(){
	
	System.out.println("Successfully executed sample method");
	}
@EnvelopeMethodProtectionAttributes(Protect = true, FeatureId = 0, Frequency = EnvelopeMethodProtectionAttributes.EnumEnvelopeMethodProtectionFrequency.CheckEveryTime, StringEncryption = EnvelopeMethodProtectionAttributes.EnumStringEncryption.SingleEncryption, CacheEncryption = 200, CodeObfuscation = true, SymbolObfuscation = false)
public static void main(String a[]){

		SampleAnnotation mat = new SampleAnnotation();
		mat.myAnnotationTestMethod();
		}

}
