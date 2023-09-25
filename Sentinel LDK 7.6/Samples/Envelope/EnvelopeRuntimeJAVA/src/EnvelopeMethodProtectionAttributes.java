import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;
import java.lang.annotation.ElementType;

@Retention(RetentionPolicy.RUNTIME)
@Target(value={ElementType.METHOD})
public @interface EnvelopeMethodProtectionAttributes{
	public enum EnumEnvelopeMethodProtectionFrequency 
	{
		CheckOncePerApplication, CheckOncePerInstance, CheckEveryTime
	} 
	public enum EnumStringEncryption
	{
		NoEncryption, SingleEncryption, MultipleEncryption
	} 

	public boolean Protect() default true;
	public int FeatureId() default -1;
	public EnumEnvelopeMethodProtectionFrequency Frequency() default EnumEnvelopeMethodProtectionFrequency.CheckOncePerApplication;
	public EnumStringEncryption StringEncryption() default EnumStringEncryption.NoEncryption; 
	public int CacheEncryption() default 300;
	public boolean CodeObfuscation() default false;
	public boolean SymbolObfuscation() default false;

}
