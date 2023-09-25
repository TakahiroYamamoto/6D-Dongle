/*****************************************************************************
*
* RUS program for protected application
*
* Copyright (C) 2015 SafeNet, Inc. All rights reserved.
*
* Please export this class to JAR file
*
*****************************************************************************/

package com.ldk_rus;

import java.lang.reflect.Field;
import java.io.BufferedReader;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileWriter;
import java.io.IOException;
import java.io.InputStreamReader;

import android.app.Activity;
import android.app.AlertDialog;
import android.app.ActionBar.LayoutParams;
import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import android.os.Environment;
import android.text.Html;
import android.text.method.ScrollingMovementMethod;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.RelativeLayout;
import android.widget.TextView;
import com.safenet.patch.Product;

public class LicenseFile extends Activity 
{
    public static final int HASP_STATUS_OK              = 0;

    /**
     * Unrecognized info format
     */
    public static final int HASP_INV_FORMAT             = 15;

    /**
     * Invalid XML scope
     */
    public static final int HASP_INV_SCOPE              = 36;

    /**
     * Specified v2c update already installed in the LLM
     */
    public static final int HASP_UPDATE_ALREADY_ADDED   = 65;

    /**
     * Trying to install a V2C file with an update counter that is out of
     * sequence with update counter in Sentinel HASP protection key. The first
     * value in the V2C file is greater than the value in Sentinel HASP
     * protection key.
     */
    public static final int HASP_UPDATE_TOO_NEW         = 55;

    /**
     * Secure storage ID mismatch
     */
    public static final int HASP_SECURE_STORE_ID_MISMATCH = 78;	

    public static final String scope = new String(
        "<haspscope>\n" +
        " <license_manager hostname=\"localhost\" />\n" +
        "</haspscope>\n");
    public static final String host_fingerprint = new String("<haspformat format=\"host_fingerprint\"/>");
    public static final String updateinfo = new String("<haspformat format=\"updateinfo\"/>");

    TextView tv;
    String filePath = null;

    private static final int REQUEST_EX = 1;

    public static String text = "";
    public static boolean fromParent = true;
    public static boolean showMsg = false;
    private static native byte[] getinfo(String scope, String format, int status[]);
    private static native String update(String update_data, int status[]);

    @Override
    public void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
        RelativeLayout layout = new RelativeLayout(this);
        super.setContentView(layout);

        final TextView title = new TextView(this);
        title.setId(1);
        title.setText(Html.fromHtml("<b>Sentinel Protection System</b><br>"));
        title.append(Html.fromHtml("Click <b>Collect Information</b> to collect system information and send it to the vendor. The vendor will send you a license update file (update.v2c).<br>"));
        title.append(Html.fromHtml("Click <b>Apply Update</b> to install your license when you receive the update.v2c file.<br>"));
        RelativeLayout.LayoutParams titleLP = new RelativeLayout.LayoutParams(LayoutParams.MATCH_PARENT,LayoutParams.WRAP_CONTENT);
        titleLP.leftMargin=10;
        titleLP.topMargin=5;

        Button c2v = new Button(this);
        c2v.setId(2);
        c2v.setText("Collect Information");
        RelativeLayout.LayoutParams c2vLP = new RelativeLayout.LayoutParams(LayoutParams.WRAP_CONTENT, LayoutParams.WRAP_CONTENT);
        c2vLP.topMargin = 16;
        c2vLP.leftMargin = 3;
        c2vLP.addRule(RelativeLayout.BELOW, 1);


        Button update  = new Button(this);
        update.setText("Apply Update");
        update.setId(3);
        RelativeLayout.LayoutParams updateLP = new RelativeLayout.LayoutParams(LayoutParams.WRAP_CONTENT, LayoutParams.WRAP_CONTENT);
        updateLP.leftMargin = 3;
        updateLP.addRule(RelativeLayout.BELOW, 2);
        //updateLP.addRule(RelativeLayout.RIGHT_OF, 2);
        //updateLP.addRule(RelativeLayout.ALIGN_BASELINE, 2);

        tv = new TextView(this);
        tv.setId(4);
        tv.setMovementMethod(new ScrollingMovementMethod());
        RelativeLayout.LayoutParams tvLP = new RelativeLayout.LayoutParams(LayoutParams.MATCH_PARENT, LayoutParams.MATCH_PARENT);
        tvLP.topMargin = 20;
        tvLP.leftMargin = 10;
        tvLP.addRule(RelativeLayout.BELOW, 3);

        layout.addView(title, titleLP);
        layout.addView(c2v, c2vLP);
        layout.addView(update, updateLP);
        layout.addView(tv, tvLP);		

        // come from parent activity, not set the text, otherwise set it
        if (fromParent)
        {
            fromParent = false;
        }
        else
        {
            tv.setText(text);
            if (showMsg)
            {
                comeback();
            }
        }

        c2v.setOnClickListener(new OnClickListener()
        {
            @Override
            public void onClick(View v) 
            {
                collectInformation();
            }
        });

        update.setOnClickListener(new OnClickListener()
        {
            @Override
            @SuppressWarnings("rawtypes")
            public void onClick(View v)
            {
                Intent intent=new Intent();
                intent.setClassName(getApplication(), "com.ldk_rus.FileBrowser");
                
                // set the field mFromParent to indicate whether the next page is start from here
                try
                {
                    Class c = Class.forName("com.ldk_rus.FileBrowser");
                    Field fromParentField = c.getField("mFromParent");
                    fromParentField.setBoolean(null, true);
                }
                catch(Exception e)
                {
                }
                
                startActivityForResult(intent, REQUEST_EX);

            }
        });
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent intent) 
    {

        if ( resultCode == RESULT_OK ) 
        {
            if ( requestCode == REQUEST_EX ) 
            {
                String filePath = intent.getStringExtra("path");

                updateLicense(filePath);
            }
        }
    }

    private void getSDcardPath()
    {              
        try 
        {
            if ( Environment.getExternalStorageState().equals( Environment.MEDIA_MOUNTED) )
            {
                File sdCardDir = Environment.getExternalStorageDirectory();
                filePath = sdCardDir.getPath();
            }
        }
        catch ( Exception e )
        {
            e.printStackTrace();
        }  
    }

    public void collectInformation()
    {
        String info;
        String format;
        int status;

        text = "Collect information: ";

        Bundle bundle = this.getIntent().getExtras();
        if ( bundle == null )
        {
            format = host_fingerprint;
        }
        else
        {
            boolean b = bundle.getBoolean("newSL", false);
            if ( b )
            {
                // to get fingerprint for install a new SL
                format = host_fingerprint;
            }
            else
            {
                // to get C2V for update an existing protection key (SL or HL)
                format = updateinfo;
            }
        }

        Product product = new Product();
        // call GetInfo to get fingerprint or c2v
        info = product.GetInfo(scope, format);
        status = product.getLastError();
        switch (status) 
        {
        case HASP_STATUS_OK:
            text += "OK\n";
            break;
        case HASP_INV_FORMAT:
            text += "Invalid format";
            break;
        case HASP_INV_SCOPE:
            text += "Invalid scope";
            break;
        default:
            text += "Failed with status:" + status;
            break;
        }

        if ( status == HASP_STATUS_OK ) 
        {
            text += writeToFile("request.c2v", info);
        } 

        tv.setText(text);
    }

    private String writeToFile(String fileName, String content)
    {
        String text = "";
        String fileFullPath = "";

        getSDcardPath();
        if ( filePath != null )
        {
            fileFullPath = filePath + "/" + fileName;
            try 
            {
                FileWriter fwriter = new FileWriter(fileFullPath);
                fwriter.write(content);
                fwriter.flush();
                fwriter.close();
                text += "Write " + fileName + " to SDCard successfully!\n" ;
            } 
            catch (IOException ex) 
            {
                text += "Write " + fileName + " to SDCard failed!\n" ;
            }
        }
        else
        {
            text += "Can't write " + fileName + " to SDCard!\n";
        }

        return text;
    }

    public void updateLicense(String filePath)
    {
        text = "";
        int status = -1;
        String v2c = null;

        v2c = readFromFile(filePath);

        if ( v2c == null )
        {
            text += "Can't load update.v2c from SD Card";
        }
        else
        {
            text += "Apply update: ";  

            Product product = new Product();
            // call Update to apply v2c
            product.Update(v2c);
            status = product.getLastError();
            switch (status)
            {
            case HASP_STATUS_OK:
                text += "OK";
                break;
            case HASP_UPDATE_ALREADY_ADDED:
                text += "Update already added";
                break;
            case HASP_UPDATE_TOO_NEW:
                text += "Update too new";
                break;
            case HASP_SECURE_STORE_ID_MISMATCH:
                text += "Secure storage ID mismatch occurred";
                break;
            default:
                text += "Failed with status:" + status;
                break;
            }
        }

        text += "\n";
        tv.setText(text);
        if(status == HASP_STATUS_OK)
        {
            comeback();
        }
    }

    private String readFromFile(String fileName) 
    {
        String content = null;

        if ( fileName != null )
        {
            File targetFile = new File(fileName);

            if ( targetFile.exists() )
            {
                try
                {
                    FileInputStream fis = new FileInputStream(fileName);
                    BufferedReader br = new BufferedReader(new InputStreamReader(fis));
                    StringBuilder sb = new StringBuilder("");
                    String line = null;

                    while ( (line = br.readLine()) != null )
                    {
                        sb.append(line);
                    }

                    br.close();

                    content = sb.toString();
                }
                catch ( Exception e )
                {
                    e.printStackTrace();
                }                
            }        
        }

        return content;
    }

    public void comeback()
    {
        showMsg = true;

        new AlertDialog.Builder(this)  
            .setTitle("Update Succeeded")
            .setMessage("Your product license has been updated successfully.\n\nClick OK to start your application.")
            .setPositiveButton("OK", new DialogInterface.OnClickListener() 
        {
            public void onClick(DialogInterface dialog, int which) 
            {
                showMsg = false;
                finish();
                ldk_rus.instance.finish();
            }
        })
            .show();
    }
}
