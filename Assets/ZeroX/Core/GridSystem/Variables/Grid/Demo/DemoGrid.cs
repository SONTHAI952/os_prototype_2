using UnityEngine;

namespace ZeroX.Variables.DemoGrid
{
    public enum TestEnemyType
    {
        EnemyA, EnemyB, EnemyC
    }
    
    [System.Serializable]
    public class TestCellA
    {
        public string name = "";
        public int age = 15;
    }
    
    [System.Serializable]
    public class TestCellB
    {
        public string name = "";
        public TestEnemyType enemyType;
        public int hp;
        public int power;
        public Color color;
        public Material material;
    }
    
    public class DemoGrid : MonoBehaviour
    {
        public Grid<TestCellA> testGridA = new Grid<TestCellA>();
        public Grid<TestCellB> testGridB = new Grid<TestCellB>();
        public Grid<string> testGridString = new Grid<string>();
    }
}