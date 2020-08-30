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
        /// �c�� (�e�X�g�p�̌Œ�l)
        /// </summary>
        private string FamilyName { get; set; }

        /// <summary>
        /// ���O (�e�X�g�p�̌Œ�l)
        /// </summary>
        private string GivenName { get; set; }

        /// <summary>
        /// �N�� (�e�X�g�p�̌Œ�l)
        /// </summary>
        private int Age { get; set; }

        /// <summary>
        /// �e�X�g�̋��ʐݒ�B
        /// </summary>
        [SetUp]
        public void Setup()
        {
            FamilyName = "�c��";
            GivenName = "���O";
            Age = 20;
        }

        /// <summary>
        /// CallBase �t���A���b�N�����\�b�h�Ȃ��B
        /// </summary>
        [Test]
        public void CallBase_True()
        {
            var humanMock = new Mock<Human>(FamilyName, GivenName, Age) { CallBase = true };

            // CallBase �� true �̂��߁A���ݒ�̃��\�b�h�͖{���̃��\�b�h���Ăяo�����
            Assert.AreEqual($"{FamilyName} {GivenName} {Age}", humanMock.Object.CreateFullNameWithAge());
        }

        /// <summary>
        /// CallBase �Ȃ��A���b�N�����\�b�h�Ȃ��B
        /// </summary>
        [Test]
        public void CallBase_False()
        {
            var humanMock = new Mock<Human>(FamilyName, GivenName, Age);

            // CallBase ���� true �̂��߁A���ݒ�̃��\�b�h�� null �ԋp�ƂȂ� (���\�b�h���̂��Ăяo����Ȃ�)
            Assert.AreEqual(null, humanMock.Object.CreateFullNameWithAge());
        }

        /// <summary>
        /// Age �v���p�e�B���㏑������B
        /// </summary>
        [Test]
        public void OverrideProperty_Age()
        {
            var humanMock = new Mock<Human>(FamilyName, GivenName, Age) { CallBase = true };
            humanMock.SetupGet(m => m.Age).Returns(9999);

            // �v���p�e�B�� get ���ԋp����Ă���
            Assert.AreEqual($"{FamilyName} {GivenName} 9999", humanMock.Object.CreateFullNameWithAge());
            Assert.AreEqual($"{FamilyName} {GivenName} 9999��", humanMock.Object.CreateFullNameWithAge("��"));
        }

        /// <summary>
        /// �����t���� CreateFullNameWithAge ���㏑������B
        /// </summary>
        [Test]
        public void OverrideMethod_CreateFullNameWithAgeUnit()
        {
            var humanMock = new Mock<Human>(FamilyName, GivenName, Age) { CallBase = true };
            humanMock.Setup(m => m.CreateFullNameWithAge(It.IsAny<string>())).Returns("�㏑��");

            // Setup �Ŏw�肵�������\���̂��̂͌Ăяo���ꂸ Returns �Őݒ肵���߂�l���ԋp����Ă���
            Assert.AreEqual("�㏑��", humanMock.Object.CreateFullNameWithAge("��"));

            // Setup �Ŏw�肵�����\�b�h�Ɠ��������A�����\�����قȂ���̂͏㏑�����ꂸ�ɏ���
            Assert.AreEqual($"{FamilyName} {GivenName} {Age}", humanMock.Object.CreateFullNameWithAge());
        }

        /// <summary>
        /// �����t���� CreateFullNameWithAge ���㏑������B
        /// </summary>
        [Test]
        public void OverrideMethod_CreateFullNameWithAgeUnit_2()
        {
            var humanMock = new Mock<Human>(FamilyName, GivenName, Age) { CallBase = true };
            humanMock.Setup(m => m.CreateFullNameWithAge("��")).Returns("�㏑��");

            // Setup �Ŏw�肵�������ݒ�Ɠ���̂��߁A�Ăяo���ꂸ Returns �Őݒ肵���߂�l���ԋp����Ă���
            Assert.AreEqual("�㏑��", humanMock.Object.CreateFullNameWithAge("��"));

            // Setup �Ŏw�肵�����\�b�h�Ɠ����E�^��̓������\�������A�ݒ�l�ƈقȂ���̂͏㏑�����ꂸ�ɏ���
            Assert.AreEqual($"{FamilyName} {GivenName} {Age}��", humanMock.Object.CreateFullNameWithAge("��"));
        }

        /// <summary>
        /// protected ���\�b�h�̃��b�N���B
        /// </summary>
        [Test]
        public void Mock_ProtectedMethod()
        {
            var humanMock = new Mock<Human>(FamilyName, GivenName, Age) { CallBase = true };
            humanMock.Protected().Setup<string>("CreateFullName").Returns("�㏑��");

            // �㏑�������߂�l���ԋp����Ă���
            Assert.AreEqual($"�㏑�� {Age}", humanMock.Object.CreateFullNameWithAge());
        }

        /// <summary>
        /// protected ���\�b�h�̎��s�B
        /// </summary>
        [Test]
        public void Run_ProtectedMethod()
        {
            var human = new Human(FamilyName, GivenName, Age);
            Type type = human.GetType();
            MethodInfo methodInfo = type.GetMethod("CreateFullName", BindingFlags.Instance | BindingFlags.NonPublic);

            // �����t�����\�b�h�̏ꍇ�A�������� object[] �^�̒l��n��
            Assert.AreEqual($"{FamilyName} {GivenName}", methodInfo.Invoke(human, null));
        }
    }
}