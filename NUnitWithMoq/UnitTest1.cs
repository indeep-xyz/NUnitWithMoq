using Moq;
using Moq.Protected;
using NUnit.Framework;
using System;
using System.Reflection;

namespace NUnitWithMoq
{
    public class Tests
    {
        /// <summary>
        /// 苗字 (テスト用の固定値)
        /// </summary>
        private string FamilyName { get; set; }

        /// <summary>
        /// 名前 (テスト用の固定値)
        /// </summary>
        private string GivenName { get; set; }

        /// <summary>
        /// 年齢 (テスト用の固定値)
        /// </summary>
        private int Age { get; set; }

        /// <summary>
        /// テストの共通設定。
        /// </summary>
        [SetUp]
        public void Setup()
        {
            FamilyName = "苗字";
            GivenName = "名前";
            Age = 20;
        }

        /// <summary>
        /// CallBase 付き、モック化メソッドなし。
        /// </summary>
        [Test]
        public void CallBase_True()
        {
            var humanMock = new Mock<Human>(FamilyName, GivenName, Age) { CallBase = true };

            // CallBase が true のため、未設定のメソッドは本来のメソッドが呼び出される
            Assert.AreEqual($"{FamilyName} {GivenName} {Age}", humanMock.Object.CreateFullNameWithAge());
        }

        /// <summary>
        /// CallBase なし、モック化メソッドなし。
        /// </summary>
        [Test]
        public void CallBase_False()
        {
            var humanMock = new Mock<Human>(FamilyName, GivenName, Age);

            // CallBase が非 true のため、未設定のメソッドは null 返却となる (メソッド自体も呼び出されない)
            Assert.AreEqual(null, humanMock.Object.CreateFullNameWithAge());
        }

        /// <summary>
        /// Age プロパティを上書きする。
        /// </summary>
        [Test]
        public void OverrideProperty_Age()
        {
            var humanMock = new Mock<Human>(FamilyName, GivenName, Age) { CallBase = true };
            humanMock.SetupGet(m => m.Age).Returns(9999);

            // プロパティの get が返却されている
            Assert.AreEqual($"{FamilyName} {GivenName} 9999", humanMock.Object.CreateFullNameWithAge());
            Assert.AreEqual($"{FamilyName} {GivenName} 9999才", humanMock.Object.CreateFullNameWithAge("才"));
        }

        /// <summary>
        /// 引数付きの CreateFullNameWithAge を上書きする。
        /// </summary>
        [Test]
        public void OverrideMethod_CreateFullNameWithAgeUnit()
        {
            var humanMock = new Mock<Human>(FamilyName, GivenName, Age) { CallBase = true };
            humanMock.Setup(m => m.CreateFullNameWithAge(It.IsAny<string>())).Returns("上書き");

            // Setup で指定した引数構成のものは呼び出されず Returns で設定した戻り値が返却されている
            Assert.AreEqual("上書き", humanMock.Object.CreateFullNameWithAge("歳"));

            // Setup で指定したメソッドと同名だが、引数構成が異なるものは上書きされずに処理
            Assert.AreEqual($"{FamilyName} {GivenName} {Age}", humanMock.Object.CreateFullNameWithAge());
        }

        /// <summary>
        /// 引数付きの CreateFullNameWithAge を上書きする。
        /// </summary>
        [Test]
        public void OverrideMethod_CreateFullNameWithAgeUnit_2()
        {
            var humanMock = new Mock<Human>(FamilyName, GivenName, Age) { CallBase = true };
            humanMock.Setup(m => m.CreateFullNameWithAge("歳")).Returns("上書き");

            // Setup で指定した引数設定と同一のため、呼び出されず Returns で設定した戻り値が返却されている
            Assert.AreEqual("上書き", humanMock.Object.CreateFullNameWithAge("歳"));

            // Setup で指定したメソッドと同名・型基準の同引数構成だが、設定値と異なるものは上書きされずに処理
            Assert.AreEqual($"{FamilyName} {GivenName} {Age}才", humanMock.Object.CreateFullNameWithAge("才"));
        }

        /// <summary>
        /// protected メソッドのモック化。
        /// </summary>
        [Test]
        public void Mock_ProtectedMethod()
        {
            var humanMock = new Mock<Human>(FamilyName, GivenName, Age) { CallBase = true };
            humanMock.Protected().Setup<string>("CreateFullName").Returns("上書き");

            // 上書きした戻り値が返却されている
            Assert.AreEqual($"上書き {Age}", humanMock.Object.CreateFullNameWithAge());
        }

        /// <summary>
        /// protected メソッドの実行。
        /// </summary>
        [Test]
        public void Run_ProtectedMethod()
        {
            var human = new Human(FamilyName, GivenName, Age);
            Type type = human.GetType();
            MethodInfo methodInfo = type.GetMethod("CreateFullName", BindingFlags.Instance | BindingFlags.NonPublic);

            // 引数付きメソッドの場合、第二引数は object[] 型の値を渡す
            Assert.AreEqual($"{FamilyName} {GivenName}", methodInfo.Invoke(human, null));
        }
    }
}