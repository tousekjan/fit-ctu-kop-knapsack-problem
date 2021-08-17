using KnapsackProblemCore.Strategies.ConstructiveStrategies;
using NUnit.Framework;

namespace KnapsackProblemTest.Constructive
{
    class KnapsackConstructiveFPTASStrategyTest
    {
        [Test]
        public void Test_NK_4()
        {
            KnapsackConstructiveTestStrategyUtils.TestOneFile(4, "NK", new KnapsackConstructiveFPTASStrategy(0.1));
        }

        [Test]
        public void Test_NK_10()
        {
            KnapsackConstructiveTestStrategyUtils.TestOneFile(10, "NK", new KnapsackConstructiveFPTASStrategy(0.2));
        }

        [Test]
        public void Test_NK_15()
        {
            KnapsackConstructiveTestStrategyUtils.TestOneFile(15, "NK", new KnapsackConstructiveFPTASStrategy(0.3));
        }

        [Test]
        public void Test_NK_20()
        {
            KnapsackConstructiveTestStrategyUtils.TestOneFile(20, "NK", new KnapsackConstructiveFPTASStrategy(0.4));
        }
    }
}
