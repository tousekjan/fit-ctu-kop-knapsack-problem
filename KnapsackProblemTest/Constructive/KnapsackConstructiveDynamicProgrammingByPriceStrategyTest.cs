using KnapsackProblemCore.Strategies.ConstructiveStrategies;
using NUnit.Framework;

namespace KnapsackProblemTest.Constructive
{
    class KnapsackConstructiveDynamicProgrammingByPriceStrategyTest
    {
        [Test]
        public void Test_NK_4()
        {
            KnapsackConstructiveTestStrategyUtils.TestOneFile(4, "NK", new KnapsackConstructiveDynamicProgrammingByPriceStrategy());
        }

        [Test]
        public void Test_NK_10()
        {
            KnapsackConstructiveTestStrategyUtils.TestOneFile(10, "NK", new KnapsackConstructiveDynamicProgrammingByPriceStrategy());
        }

        [Test]
        public void Test_NK_15()
        {
            KnapsackConstructiveTestStrategyUtils.TestOneFile(15, "NK", new KnapsackConstructiveDynamicProgrammingByPriceStrategy());
        }

        [Test]
        public void Test_NK_20()
        {
            KnapsackConstructiveTestStrategyUtils.TestOneFile(20, "NK", new KnapsackConstructiveDynamicProgrammingByPriceStrategy());
        }
    }
}
