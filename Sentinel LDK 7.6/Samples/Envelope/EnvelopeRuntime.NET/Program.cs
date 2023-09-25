using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Aladdin.HASP.Envelope;
using Aladdin.HASP.EnvelopeRuntime;

// By default do not protect any methods in this assembly.
[assembly: EnvelopeMethodProtectionAttributes(Protect = false)]

namespace Sample
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SampleForm());
        }
    }

    // Default protection parameters for all methods in this class.
    // Enable protection, encryption and code obfuscation for all methods.
    [EnvelopeMethodProtectionAttributes(Protect = true, Encrypt = true, CodeObfuscation = true, TreatCheckOnlyAsUnprotectable=true)]
    class Calculator
    {
        // Do not protect .ctor
        [EnvelopeMethodProtectionAttributes(Protect = false)]
        public Calculator()
        { 
        }
        
        // Protection Add method with Feature 51, check every time.
        [EnvelopeMethodProtectionAttributes(FeatureId = 51, Frequency = EnvelopeMethodProtectionFrequency.CheckEveryTime)]
        public int Add(int a, int b)
        {
            return a + b;
        }

        // Protect Subtract method with Feature 52, no encryption, check every time.
        [EnvelopeMethodProtectionAttributes(FeatureId = 52, Encrypt = false, Frequency = EnvelopeMethodProtectionFrequency.CheckEveryTime)]
        public int Subtract(int a, int b)
        {
            return a - b;
        }
        
        // Protect Multiply method with Feature 53, no code obfuscation, check every time.
        [EnvelopeMethodProtectionAttributes(FeatureId = 53, CodeObfuscation = false, Frequency = EnvelopeMethodProtectionFrequency.CheckEveryTime)]
        public Int64 Multiply(int a, int b)
        {
            return (Int64)a * (Int64)b;
        }

        // Protect Divide method with Feature 54, check only once per application.
        [EnvelopeMethodProtectionAttributes(FeatureId = 54, Frequency = EnvelopeMethodProtectionFrequency.CheckOncePerApplicaton)]
        public int Divide(int a, int b)
        {
            return a / b;
        }
    }
}

