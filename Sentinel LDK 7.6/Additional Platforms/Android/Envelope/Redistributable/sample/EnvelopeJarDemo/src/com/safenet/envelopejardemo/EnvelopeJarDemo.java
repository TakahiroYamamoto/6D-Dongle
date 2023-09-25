package com.safenet.envelopejardemo;

import android.app.Activity;
import android.os.Bundle;
import android.util.Log; 
import android.content.Intent;
import android.widget.TextView;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.LinearLayout;
import android.view.ViewGroup.LayoutParams;
import android.view.Gravity;

import com.safenet.patch.SfntLicense;

public class EnvelopeJarDemo extends Activity 
{
	private final String LOG_TAG = "EnvelopeDemo";
	private final String TITLE_MSG = "Sample Application to show usage of android_rt.jar";
	private Button getLicBtn = null;
	private Button loadLibBtn = null;
	private Button closeBtn = null;
	private TextView tvMsg = null;
	private TextView tvStatus = null;
	private final String TAG1 = "BTN_GETLIC";
	private final String TAG2 = "BTN_LOADLIB";
	private final String TAG3 = "BTN_CLOSE";
	
	private String vendorCode = new String(
        "AzIceaqfA1hX5wS+M8cGnYh5ceevUnOZIzJBbXFD6dgf3tBkb9cvUF/Tkd/iKu2fsg9wAysYKw7RMA" +
        "sVvIp4KcXle/v1RaXrLVnNBJ2H2DmrbUMOZbQUFXe698qmJsqNpLXRA367xpZ54i8kC5DTXwDhfxWT" +
        "OZrBrh5sRKHcoVLumztIQjgWh37AzmSd1bLOfUGI0xjAL9zJWO3fRaeB0NS2KlmoKaVT5Y04zZEc06" +
        "waU2r6AU2Dc4uipJqJmObqKM+tfNKAS0rZr5IudRiC7pUwnmtaHRe5fgSI8M7yvypvm+13Wm4Gwd4V" +
        "nYiZvSxf8ImN3ZOG9wEzfyMIlH2+rKPUVHI+igsqla0Wd9m7ZUR9vFotj1uYV0OzG7hX0+huN2E/Id" +
        "gLDjbiapj1e2fKHrMmGFaIvI6xzzJIQJF9GiRZ7+0jNFLKSyzX/K3JAyFrIPObfwM+y+zAgE1sWcZ1" +
        "YnuBhICyRHBhaJDKIZL8MywrEfB2yF+R3k9wFG1oN48gSLyfrfEKuB/qgNp+BeTruWUk0AwRE9XVMU" +
        "uRbjpxa4YA67SKunFEgFGgUfHBeHJTivvUl0u4Dki1UKAT973P+nXy2O0u239If/kRpNUVhMg8kpk7" +
        "s8i6Arp7l/705/bLCx4kN5hHHSXIqkiG9tHdeNV8VYo5+72hgaCx3/uVoVLmtvxbOIvo120uTJbuLV" +
        "TvT8KtsOlb3DxwUrwLzaEMoAQAFk6Q9bNipHxfkRQER4kR7IYTMzSoW5mxh3H9O8Ge5BqVeYMEW36q" +
        "9wnOYfxOLNw6yQMf8f9sJN4KhZty02xm707S7VEfJJ1KNq7b5pP/3RjE0IKtB2gE6vAPRvRLzEohu0" +
        "m7q1aUp8wAvSiqjZy7FLaTtLEApXYvLvz6PEJdj4TegCZugj7c8bIOEqLXmloZ6EgVnjQ7/ttys7VF" +
        "ITB3mazzFiyQuKf4J6+b/a/Y");
	
	private int featureId = 100;	
	
	native int GetRandomNumber();
	
   /** Called when the activity is first created. */
	@Override
	public void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);
		
		drawScreen();
	}


	private void drawScreen() 
	{ 		
		LayoutParams params = new LinearLayout.LayoutParams(
                    LayoutParams.WRAP_CONTENT,
                    LayoutParams.WRAP_CONTENT);
										
		tvMsg = new TextView(this);
		tvMsg.setText(TITLE_MSG);
		tvMsg.setPadding(0,0,0,35);
		tvMsg.setTextSize(20);
		tvMsg.setLayoutParams(params);

		tvStatus = new TextView(this);
		tvStatus.setText("");
		tvStatus.setPadding(0,0,0,35);
		tvStatus.setTextSize(16);
		tvStatus.setLayoutParams(params);
		
		getLicBtn = new Button(this);
		getLicBtn.setText("Get License");
		getLicBtn.setTag(TAG1);
		getLicBtn.setLayoutParams(params);
		
		View.OnClickListener myhandler1 = new View.OnClickListener() {
			public void onClick(View v) {		
				// Set vendor code and feature id
				SfntLicense.m_VendorCode = vendorCode;
				SfntLicense.m_FeatureId = featureId;
				try{
					Intent mainClassIntent = new Intent(getApplicationContext(), Class.forName("com.safenet.patch.SfntLicense"));
					startActivityForResult(mainClassIntent, 1); 
					
				}catch (Exception exception) {
						// Handle exception gracefully here.
					System.out.println(exception.getMessage());		
					tvStatus.setText(exception.getMessage());
				}
			}
		};
		getLicBtn.setOnClickListener(myhandler1);
		
		loadLibBtn = new Button(this);
		loadLibBtn.setText("Load Protected Shared Object");
		loadLibBtn.setTag(TAG2);
		loadLibBtn.setLayoutParams(params);
		
		View.OnClickListener myhandler2 = new View.OnClickListener() {
			public void onClick(View v) {
				// it was the 2nd button
				try{	
					System.loadLibrary("random");
					tvStatus.setText("Library loaded successfully.");
				}
				catch (UnsatisfiedLinkError exception) {
					Log.i("Sample", "Exception>>:" + exception.getMessage());		
					exception.printStackTrace();
					tvStatus.setText("Shared Object not found.");
				}
				catch (Exception ex) {
						// Handle exception gracefully here.
					System.out.println(ex.getMessage());		
					tvStatus.setText(ex.getMessage());
				}
				tvStatus.append("Random Number: " + GetRandomNumber());
				
			}
		};
		loadLibBtn.setOnClickListener(myhandler2);

		
		closeBtn = new Button(this);
		closeBtn.setText("  Exit  ");
		closeBtn.setTag(TAG3);
		closeBtn.setLayoutParams(params);
		View.OnClickListener myhandler3 = new View.OnClickListener() {
			public void onClick(View v) {
				// Close the application
				finish();
			}
		};
		closeBtn.setOnClickListener(myhandler3);
		
		LinearLayout lyout = new LinearLayout(this);
		lyout.setOrientation(LinearLayout.VERTICAL);
		lyout.setPadding(0,50,0,0);
		lyout.setHorizontalGravity(Gravity.CENTER);
				
		lyout.addView(tvMsg);		
		lyout.addView(getLicBtn);
		lyout.addView(loadLibBtn);
		lyout.addView(closeBtn);
		lyout.addView(tvStatus);
		
		
		Activity act = (Activity) this;
		act.addContentView(lyout, params);		
	}
	
	public void onActivityResult(int requestCode, int resultCode, Intent data)
	{		
		if(resultCode != SfntLicense.RESULT_OK)
		{
			if(resultCode == SfntLicense.RESULT_ALREADY_INSTALLED)
				tvStatus.setText("License already installed");
			else if(resultCode == SfntLicense.RESULT_FAILED)
				tvStatus.setText("License installation failed.");
		}
	}
	
	
}
