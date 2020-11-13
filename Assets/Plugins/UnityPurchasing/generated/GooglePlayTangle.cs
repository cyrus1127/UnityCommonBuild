#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("HZ6Qn68dnpWdHZ6enyDmx9c24oywhR1gWuHKeabIUymfu4Oc0NTn5hSbTEwjboWpZ06P18otMOTzOrNKmKVGsqeSVGLHTa9DyH/q6el91+KvHZ69r5KZlrUZ1xlokp6enpqfnE4nOSDG7KDt68cxI39wBCZHxUdr5Q6ZjrDm6Z75kf541+PLPlcjb54HOqG3EQIo/axA7SrGWj4BCBwAkMWg2ZVZvjcdTiyhtj5dqi3lRdm6Ldp+MYDMJN+bOrbTBC/9lc8ZcVApIFcO2uiK/Z8CVci5cxEaWeKJjV8EQGlUjE97xBxAQqwWh5sJ/q6SxF4UClv+czoI/fORWhahli7r8gJOvU5LpcqLoVCd7LdpUqKSLs5FVNAZV69Wzq3Olp2cnp+e");
        private static int[] order = new int[] { 1,9,5,12,9,5,7,12,10,11,13,13,12,13,14 };
        private static int key = 159;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
