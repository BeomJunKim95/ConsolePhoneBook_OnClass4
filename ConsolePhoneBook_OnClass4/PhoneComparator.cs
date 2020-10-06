using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePhoneBook
{
    //class PhoneComparator : IComparer
    class PhoneComparator : IComparer<PhoneInfo> //List의 T를 정렬하려면 인터페이스도 <>로 바꿔줘야함 
    {
        //배열로 만들었을때는 오브젝트로 해서 어떤타입이 올지 몰랐지만
        //public int Compare(object x, object y)
        //{
        //    PhoneInfo first = (PhoneInfo)x;
        //    PhoneInfo other = (PhoneInfo)y;

        //    return first.Phone.CompareTo(other.Phone);
        //}

        //List를 사용했을땐 PhoneInfo타입만 비교를 하겠다는거니까 타입이 정해짐
		public int Compare(PhoneInfo x, PhoneInfo y)
		{
            //이젠 타입이 정해져 있으니 형변환이 필요없음
            return x.Phone.CompareTo(y.Phone);
		}

		//public int Compare(PhoneInfo x, PhoneInfo y)
		//{
		//    return x.Phone.CompareTo(y.Phone);
		//}
	}

    //class NameComparator : IComparer
        class NameComparator : IComparer<PhoneInfo>
    {
        //public int Compare(object x, object y)
        //{
        //    PhoneInfo first = (PhoneInfo)x;
        //    PhoneInfo other = (PhoneInfo)y;

        //    return first.Name.CompareTo(other.Name);
        //}

		public int Compare(PhoneInfo x, PhoneInfo y)
		{
            return x.Name.CompareTo(y.Name);
		}
	}
}
