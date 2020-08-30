namespace NUnitWithMoq
{
    /// <summary>
    /// �e�X�g�ΏۃN���X�B
    /// </summary>
    public class Human
    {
    /// <summary>
    /// �c��
    /// </summary>
    private string FamilyName { get; }

    /// <summary>
    /// ���O
    /// </summary>
    private string GivenName { get; }

    /// <summary>
    /// �N��
    /// </summary>
    public virtual int Age { get; }

        /// <summary>
        /// �R���X�g���N�^�B
        /// </summary>
        /// <param name="familyName">�c��</param>
        /// <param name="givenName">���O</param>
        /// <param name="age">�N��</param>
        public Human(string familyName, string givenName, int age)
        {
            FamilyName = familyName;
            GivenName = givenName;
            Age = age;
        }

        /// <summary>
        /// �t���l�[�����쐬����B
        /// </summary>
        /// <returns>�t���l�[��</returns>
        protected virtual string CreateFullName()
        {
            return $"{FamilyName} {GivenName}";
        }

        /// <summary>
        /// �N��t���̃t���l�[�����쐬����B
        /// </summary>
        /// <returns>�N��t���̃t���l�[��</returns>
        public virtual string CreateFullNameWithAge()
        {
            return $"{CreateFullName()} {Age}";
        }

        /// <summary>
        /// �N��Ƃ��̒P�ʕt���̃t���l�[�����쐬����B
        /// </summary>
        /// <returns>�N��Ƃ��̒P�ʕt���̃t���l�[��</returns>
        public virtual string CreateFullNameWithAge(string ageUnit)
        {
            return $"{CreateFullNameWithAge()}{ageUnit}";
        }
    }
}