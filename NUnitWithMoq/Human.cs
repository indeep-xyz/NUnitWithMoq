namespace NUnitWithMoq
{
    /// <summary>
    /// テスト対象クラス。
    /// </summary>
    public class Human
    {
    /// <summary>
    /// 苗字
    /// </summary>
    private string FamilyName { get; }

    /// <summary>
    /// 名前
    /// </summary>
    private string GivenName { get; }

    /// <summary>
    /// 年齢
    /// </summary>
    public virtual int Age { get; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="familyName">苗字</param>
        /// <param name="givenName">名前</param>
        /// <param name="age">年齢</param>
        public Human(string familyName, string givenName, int age)
        {
            FamilyName = familyName;
            GivenName = givenName;
            Age = age;
        }

        /// <summary>
        /// フルネームを作成する。
        /// </summary>
        /// <returns>フルネーム</returns>
        protected virtual string CreateFullName()
        {
            return $"{FamilyName} {GivenName}";
        }

        /// <summary>
        /// 年齢付きのフルネームを作成する。
        /// </summary>
        /// <returns>年齢付きのフルネーム</returns>
        public virtual string CreateFullNameWithAge()
        {
            return $"{CreateFullName()} {Age}";
        }

        /// <summary>
        /// 年齢とその単位付きのフルネームを作成する。
        /// </summary>
        /// <returns>年齢とその単位付きのフルネーム</returns>
        public virtual string CreateFullNameWithAge(string ageUnit)
        {
            return $"{CreateFullNameWithAge()}{ageUnit}";
        }
    }
}