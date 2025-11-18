using UnityEngine;

namespace ZeroX.Editors.Demo
{
    public enum DemoOption
    {
        A, B, C, D, E, F, G ,H
    }
    
    public class DemoZEditor : MonoBehaviour
    {
        public DemoOption option = DemoOption.A;
        
        [ShowFieldIf("option", DemoOption.A)]
        public string aValue;
        
        [HideFieldIf("option", DemoOption.B)]
        public string bValue;
        
        [EnableFieldIf("option", DemoOption.C)]
        public string cValue;
        
        [DisableFieldIf("option", DemoOption.D)]
        public string dValue;
        
        // [ShowFieldIf("option", DemoOption.E)]
        // public string eValue; 
        //
        // [ShowFieldIf("option", DemoOption.F)]
        // public string fValue; 
        //
        // [ShowFieldIf("option", DemoOption.G)]
        // public string gValue; 
        //
        // [ShowFieldIf("option", DemoOption.H)]
        // public string hValue; 
        
        [Header("----------")]
        public bool useRigidbody = false;
        
        [ShowFieldIf("useRigidbody")]
        public Rigidbody rigidbody;

        public int t1;
        public int t2;
        public int t3;


        [ContextMenu("Test")]
        public void Test()
        {
           
            
        }

    }
}