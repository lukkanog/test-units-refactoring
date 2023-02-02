using Store.Domain.Entities;
using Store.Domain.Enums;

namespace Store.Tests 
{
    [TestClass]
    public class OrderTests
    {
        private readonly Customer _customer = new Customer("Bruce Wayne", "iamnotbatman@email.com");
        private readonly Discount _discount = new Discount(10, DateTime.Now.AddDays(5));
        private readonly Product _mouse = new Product("Mouse", 299, true);

        private Order _validOrder { get; set; }
        
        public OrderTests()
        {
            _validOrder = new Order(_customer, 10, _discount);
        }

        [TestMethod]
        [TestCategory("Unit - Domain")]
        public void WhenNewOrderIsValid_ShouldGenerateNumberWithEightCharacters()
            => Assert.AreEqual(8, _validOrder.Number.Length);
        

        [TestMethod]
        [TestCategory("Unit - Domain")]
        public void WhenNewOrderIsValid_ShouldHaveWaitingPaymentStatus()
            => Assert.AreEqual(EOrderStatus.WaitingPayment, _validOrder.Status);        

        [TestMethod]
        [TestCategory("Unit - Domain")]
        public void WhenOrderHasPayment_ShouldHaveWaitingDeliverStatus()
        {
            _validOrder.AddItem(_mouse, 1);
            _validOrder.Pay(299);

            Assert.AreEqual(EOrderStatus.WaitingDelivery, _validOrder.Status);
        }

        [TestMethod]
        [TestCategory("Unit - Domain")]
        public void WhenOrderIsCanceled_ShouldHaveCanceledStatus()
        {
            _validOrder.AddItem(_mouse, 1);
            _validOrder.Cancel();

            Assert.AreEqual(EOrderStatus.Canceled, _validOrder.Status);
        }

        [TestMethod]
        [TestCategory("Unit - Domain")]
        public void WhenItemIsAddedWithNullProduct_ShouldNotAddItem()
        {
            _validOrder.AddItem(null, 1);

            Assert.AreEqual(0, _validOrder.Items.Count);
        }

        [TestMethod]
        [TestCategory("Unit - Domain")]
        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void WhenItemIsAddedWithZeroOrLessQuantity_ShouldNotAddItem(int quantity)
        {
            _validOrder.AddItem(_mouse, quantity);
            Assert.AreEqual(0, _validOrder.Items.Count);

        }

        [TestMethod]
        [TestCategory("Unit - Domain")]
        public void WhenOrderIsValid_ShouldHaveTotalAs50()
        {
            var product = new Product("Mouse", 50, true);
            var order = new Order(_customer, 10, _discount);

            order.AddItem(product, 1);
            Assert.AreEqual(50, order.Total());
        }

        [TestMethod]
        [TestCategory("Unit - Domain")]
        public void WhenDiscountIsExpired_ShouldNotApplyDiscount()
        {
            var discount = new Discount(10, DateTime.Now.AddDays(-5));
            var order = new Order(_customer, 0, discount);
            order.AddItem(_mouse, 1);

            Assert.AreEqual(299, order.Total());
        }

        [TestMethod]
        [TestCategory("Unit - Domain")]
        public void WhenDiscountIsNull_ShouldNotApplyDiscount()
        {
            var order = new Order(_customer, 0, null);
            order.AddItem(_mouse, 1);

            Assert.AreEqual(299, order.Total());
        }

        [TestMethod]
        [TestCategory("Unit - Domain")]
        public void WhenDiscountIsApplied_ShouldHaveTotalAs289()
        {
            var order = new Order(_customer, 0, _discount);
            order.AddItem(_mouse, 1);

            Assert.AreEqual(289, order.Total());
        }

        [TestMethod]
        [TestCategory("Unit - Domain")]
        public void WhenDeliveryFeeIsValid_ShouldApplyDeliveryFee()
        {
            var order = new Order(_customer, 10, null);
            order.AddItem(_mouse, 1);

            Assert.AreEqual(309, order.Total());
        }

        [TestMethod]
        [TestCategory("Unit - Domain")]
        public void WhenUserIsInvalid_ShouldNotCreateOrder()
        {
            var order = new Order(null, 10, null);
            Assert.AreEqual(false, order.IsValid);
        }

    }
}