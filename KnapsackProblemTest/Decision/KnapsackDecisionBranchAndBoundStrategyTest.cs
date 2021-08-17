using KnapsackProblemCore.Strategies.DecisionStrategies;
using NUnit.Framework;

namespace KnapsackProblemTest.Decision
{
    public class KnapsackDecisionBranchAndBoundStrategyTest
    {
        #region NR
        [Test]
        public void Test_NR_4()
        {
            KnapsackDecisionTestStrategyUtils.TestOneFile(4, "NR", new KnapsackDecisionBranchAndBoundStrategy());
        }

        [Test]
        public void Test_NR_10()
        {
            KnapsackDecisionTestStrategyUtils.TestOneFile(10, "NR", new KnapsackDecisionBranchAndBoundStrategy());
        }

        [Test]
        public void Test_NR_15()
        {
            KnapsackDecisionTestStrategyUtils.TestOneFile(15, "NR", new KnapsackDecisionBranchAndBoundStrategy());
        }

        [Test]
        public void Test_NR_22()
        {
            KnapsackDecisionTestStrategyUtils.TestOneFile(22, "NR", new KnapsackDecisionBranchAndBoundStrategy());
        }

        [Test]
        public void Test_NR_25()
        {
            KnapsackDecisionTestStrategyUtils.TestOneFile(25, "NR", new KnapsackDecisionBranchAndBoundStrategy());
        }

        [Test]
        public void Test_NR_27()
        {
            KnapsackDecisionTestStrategyUtils.TestOneFile(27, "NR", new KnapsackDecisionBranchAndBoundStrategy());
        }

        //[Test]
        public void Test_NR_30()
        {
            KnapsackDecisionTestStrategyUtils.TestOneFile(30, "NR", new KnapsackDecisionBranchAndBoundStrategy());
        }

        //[Test]
        public void Test_NR_32()
        {
            KnapsackDecisionTestStrategyUtils.TestOneFile(32, "NR", new KnapsackDecisionBranchAndBoundStrategy());
        }

        #endregion
        #region ZR
        [Test]
        public void Test_ZR_4()
        {
            KnapsackDecisionTestStrategyUtils.TestOneFile(4, "ZR", new KnapsackDecisionBranchAndBoundStrategy());
        }

        [Test]
        public void Test_ZR_10()
        {
            KnapsackDecisionTestStrategyUtils.TestOneFile(10, "ZR", new KnapsackDecisionBranchAndBoundStrategy());
        }

        [Test]
        public void Test_ZR_15()
        {
            KnapsackDecisionTestStrategyUtils.TestOneFile(15, "ZR", new KnapsackDecisionBranchAndBoundStrategy());
        }

        [Test]
        public void Test_ZR_22()
        {
            KnapsackDecisionTestStrategyUtils.TestOneFile(22, "ZR", new KnapsackDecisionBranchAndBoundStrategy());
        }

        [Test]
        public void Test_ZR_25()
        {
            KnapsackDecisionTestStrategyUtils.TestOneFile(25, "ZR", new KnapsackDecisionBranchAndBoundStrategy());
        }

        [Test]
        public void Test_ZR_27()
        {
            KnapsackDecisionTestStrategyUtils.TestOneFile(27, "NR", new KnapsackDecisionBranchAndBoundStrategy());
        }

        //[Test]
        public void Test_ZR_30()
        {
            KnapsackDecisionTestStrategyUtils.TestOneFile(30, "ZR", new KnapsackDecisionBranchAndBoundStrategy());
        }

        //[Test]
        public void Test_ZR_32()
        {
            KnapsackDecisionTestStrategyUtils.TestOneFile(32, "ZR", new KnapsackDecisionBranchAndBoundStrategy());
        }

        #endregion
    }
}
