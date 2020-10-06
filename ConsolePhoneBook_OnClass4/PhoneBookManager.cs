using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePhoneBook
{
    /// <summary>
    /// PhoneBookManager : 이 클래스의 역할
    /// 작성자 : 
    /// 최초작성일 : 
    /// 수정내용 : 
    /// </summary>
    public class PhoneBookManager
    {
        //const int MAX_CNT = 100; 
        //PhoneInfo[] infoStorage = new PhoneInfo[MAX_CNT]; //HashSet을 쓰면 배열관련 멤버들은 이제 필요가없음
        //int curCnt = 0;

        HashSet<PhoneInfo> infoStorage = new HashSet<PhoneInfo>();
        static PhoneBookManager inst = null;
        readonly string dataFile = "PhoneBook.dat"; //파일명 고정을 위한 dataFile변수

        public void ShowMenu()
        {
            Console.WriteLine("------------------------ 주소록 -----------------------------------");
            Console.WriteLine("1. 입력  |  2. 목록  |  3. 검색  |  4. 정렬  |  5. 삭제  |  6. 종료");
            Console.WriteLine("------------------------------------------------------------------");
            Console.Write("선택: ");
        }

        // 싱글톤 (싱글톤의 공식같은 코드)
		private PhoneBookManager()
        // 함부러 인스턴스 new를 하지못하게 기본생성자를 public이 아니라 private으로
        //반드시 기본생성자를 밖에서 부르지 못하게 private으로 둬야함
        {
            ReadToFile(); //생성자에서 불러도 되고 Program의 반복문 밖에 선언해도댐
        }

		private void ReadToFile() //내부에서 부르기땜에 프라이빗
		{
            if (!File.Exists(dataFile)) //처음에 읽을 파일이 없을경우 return
                return;

			try
			{
                FileStream fs = new FileStream(dataFile, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                infoStorage.Clear(); // 안전하게 클리하고 주는게 안전
                
                infoStorage = (HashSet<PhoneInfo>)formatter.Deserialize(fs); //그냥은 안돼니 infostrage와 같은 타입인 HashSet으로 형변환
                fs.Close();
			}
			catch(IOException err)
			{
				Console.WriteLine(err.Message);
			}
            catch(Exception err) //혹시 모르겠으면 다중캐치문 써도 괜찮음
			{
				Console.WriteLine(err.Message);
			}
		}
        public void WriteToFile() // 밖에서 부르니 Public
        {
            //try~catch를 써야할때중 하나인 파일을 읽고쓸때
			try
			{
                using (FileStream fs = new FileStream(dataFile, FileMode.Create)) //덮어쓰기모드로
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, infoStorage);
                }
                //fs.Close(); //클로즈를 꼭써야하는데 하기싫으면 Using으로 묶으면 된다
			}
            catch (IOException err) //내 오류메세지를 참조하기 위해 ()사용
                                    //여기서 난 오류는 입출력밖에 없기 때문에 IOException사용가능
                                    //하지만 IOException이 아니면 예외처리를 하지 않기 때문에 신중히 결정해야함
            {
                Console.WriteLine(err.Message);
			}
        }

        public static PhoneBookManager CreateManagerInstance() //인스턴스를 만들어주는 메서드 생성
        //인스턴스를 생성할수 없기 때문에 클래스명으로 호출해야하니 정적메서드로
		{
            //메서드가 1개이상이면 인스턴스를 만들지않음
            if (inst == null)
               inst = new PhoneBookManager();

            return inst;
		}

        public void SortData()
        {            
            int choice;
            while (true)
            {
                try
                {
                    Console.WriteLine("1.이름 ASC  2.이름 DESC  3.전화번호 ASC  4.전화번호 DESC");
                    Console.Write("선택 >> ");

                    if (int.TryParse(Console.ReadLine(), out choice))
                    {
                        if (choice < 1 || choice > 4)
                        {
                            throw new MenuChoiceException(choice);

                            //Console.WriteLine("1.이름 ASC  2.이름 DESC  3.전화번호 ASC  4.전화번호 DESC 중에 선택하십시오.");
                            //return;
                        }
                        else
                            break;
                    }
                }
                catch(MenuChoiceException err)
                {
                    err.ShowWrongChoice();
                }
            }
            //배열은 이제 필요없음
            //PhoneInfo[] new_arr = new PhoneInfo[curCnt];
            //Array.Copy(infoStorage, new_arr, curCnt);

            List<PhoneInfo> list = new List<PhoneInfo>(infoStorage); //List에 3개의 생성자가 있는데
            //1.아무것도 안넣는것   2. 초기용량을 정해주는것   3. 컬렉션을 넣어주는것   지금은 3번 사용


            if (choice == 1)
            {
                // Array.Sort(new_arr, new NameComparator());
                list.Sort(new NameComparator());
            }
            else if (choice == 2)
            {
                //Array.Sort(new_arr, new NameComparator());
                //Array.Reverse(new_arr);
                list.Sort(new NameComparator());
                list.Reverse();
            }
            else if (choice == 3)
            {
                //Array.Sort(new_arr, new PhoneComparator());
                list.Sort(new PhoneComparator()); // sort메서드에 컴패어레이터를 인자로 주는것도 가능
            }
            else if (choice == 4)
            {
                //Array.Sort(new_arr, new PhoneComparator());
                //Array.Reverse(new_arr);
                list.Sort(new PhoneComparator());
                list.Reverse();
            }

            //for (int i = 0; i < curCnt; i++)
            //{
            //    Console.WriteLine(new_arr[i].ToString());
            //}
            foreach(PhoneInfo info in list)
			{
                info.ShowPhoneInfo();
				Console.WriteLine();
			}
        }

		public void InputData()
        {
            Console.WriteLine("1.일반  2.대학  3.회사");
            Console.Write("선택 >> ");
            int choice;
            while (true)
            {                
                if (int.TryParse(Console.ReadLine(), out choice))
                    break;
            }
            if (choice < 1 || choice > 3)
            {
                Console.WriteLine("1.일반  2.대학  3.회사 중에 선택하십시오.");
                return;
            }

            PhoneInfo info = null;
            switch(choice)
            {
                case 1:
                    info = InputFriendInfo();
                    break;
                case 2:
                    info = InputUnivInfo(); 
                    break;
                case 3:
                    info = InputCompanyInfo(); 
                    break;
            }
            if (info != null)
            {
                //infoStorage[curCnt++] = info;

                bool isAdded = infoStorage.Add(info); //컬렉션 HashSet사용
                                       //Add가 bool타입인 이유는 HashSet특성이 같은값이 들어오면 무시하고 다른값이 들어와야 처리해주기때문
                if(isAdded) // 그래서 실효성체크를 해주는 좋은코딩을 하자
					Console.WriteLine("데이터 입력이 완료되었습니다");
                else
					Console.WriteLine("이미 저장된 데이터입니다.");
            }
        }

        private List<string> /*string[]*/ InputCommonInfo()
        {
            Console.Write("이름: ");
            string name = Console.ReadLine().Trim();
            //if (name == "") or if (name.Length < 1) or if (name.Equals(""))
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("이름은 필수입력입니다");
                return null;
            }
            else
            {
                //int dataIdx = SearchName(name);
                if (/*dataIdx > -1*/SearchName(name))
                {
                    Console.WriteLine("이미 등록된 이름입니다. 다른 이름으로 입력하세요");
                    return null;
                }
            }

            Console.Write("전화번호: ");
            string phone = Console.ReadLine().Trim();
            if (string.IsNullOrEmpty(phone))
            {
                Console.WriteLine("전화번호는 필수입력입니다");
                return null;
            }

            Console.Write("생일: ");
            string birth = Console.ReadLine().Trim();

            //string[] arr = new string[3];
            //arr[0] = name;
            //arr[1] = phone;
            //arr[2] = birth;

            //return arr;

            List<string> list = new List<string>();
            list.Add(name);
            list.Add(phone);
            list.Add(birth);

            return list;
        }   
            
        private PhoneInfo InputFriendInfo()
        {
            List<string>/*string[]*/ comInfo = InputCommonInfo();
            if (comInfo == null || comInfo.Count/*.Length*/ != 3)
                return null;

            return new PhoneInfo(comInfo[0], comInfo[1], comInfo[2]);
        }

        private PhoneInfo InputUnivInfo()
        {
            List<string>/*string[]*/ comInfo = InputCommonInfo();
            if (comInfo == null || comInfo.Count/*.Length*/ != 3)
                return null;

            Console.Write("전공: ");
            string major = Console.ReadLine().Trim();

            Console.Write("학년: ");
            int year = int.Parse(Console.ReadLine().Trim());

            return new PhoneUnivInfo(comInfo[0], comInfo[1], comInfo[2], major, year);
        }

        private PhoneInfo InputCompanyInfo()
        {
            List<string>/*string[]*/ comInfo = InputCommonInfo();
            if (comInfo == null || comInfo.Count/*.Length*/ != 3)
                return null;

            Console.Write("회사명: ");
            string company = Console.ReadLine().Trim();

            return new PhoneCompanyInfo(comInfo[0], comInfo[1], comInfo[2], company);
        }

        public void ListData()
        {
            if (/*curCnt*/infoStorage.Count == 0)
            {
                Console.WriteLine("입력된 데이터가 없습니다.");
                return;
            }
            foreach(PhoneInfo info in infoStorage)
			{
                info.ShowPhoneInfo();
				Console.WriteLine();
			}
            //HashSet은 인덱스 접근이 안되서 아래코드는 필요가없음
            //for(int i=0; i<curCnt; i++)
            //{
            //    //infoStorage[i].ShowPhoneInfo();
            //    //Console.WriteLine();

            //    Console.WriteLine(infoStorage[i].ToString());                
            //}
        }
        #region 배열과 HashSet SearchData
        public void SearchData()
		{
			Console.WriteLine("주소록 검색을 시작합니다......");
            PhoneInfo findInfo = SearchName();
            if(findInfo == null)
			{
				Console.WriteLine("검색된 데이터가 없습니다");
                return;
			}
            else
			{
                findInfo.ShowPhoneInfo();
                Console.WriteLine();
			}
		}
		#region 배열 SearchData
		//public void SearchData()
		//{
		//    Console.WriteLine("주소록 검색을 시작합니다......");
		//    int dataIdx = SearchName();
		//    if (dataIdx < 0)
		//    {
		//        Console.WriteLine("검색된 데이터가 없습니다");
		//    }
		//    else
		//    {
		//        infoStorage[dataIdx].ShowPhoneInfo();
		//        Console.WriteLine();
		//    }

		//    #region 모두 찾기
		//    //int findCnt = 0;
		//    //for(int i=0; i<curCnt; i++)
		//    //{
		//    //    // ==, Equals(), CompareTo()
		//    //    if (infoStorage[i].Name.Replace(" ","").CompareTo(name) == 0)
		//    //    {
		//    //        infoStorage[i].ShowPhoneInfo();
		//    //        findCnt++;
		//    //    }
		//    //}
		//    //if (findCnt < 1)
		//    //{
		//    //    Console.WriteLine("검색된 데이터가 없습니다");
		//    //}
		//    //else
		//    //{
		//    //    Console.WriteLine($"총 {findCnt} 명이 검색되었습니다.");
		//    //}
		//    #endregion
		//}
		#endregion
		#endregion

		#region 배열과 HashSet SearchName
		private PhoneInfo SearchName()
		{
            //PhoneInfo findInfo = null; //먼저 널값을 주고

			Console.Write("이름 : ");
            string name = Console.ReadLine().Trim().Replace(" ", "");

            foreach(PhoneInfo info in infoStorage)
			{
                if(name.CompareTo(info.Name)==0)
				{
                    //findInfo = info; // 같은이름을 찾다가 있으면 반환해주고 없으면 null값 리턴
                    //break;
                    return info; // 이렇게 코드를 줄일수도 있다
				}
			}
            //return findInfo;
            return null;
        }
        #region 배열 SearchName
        //private int SearchName()
        //{
        //	Console.Write("이름: ");
        //	string name = Console.ReadLine().Trim().Replace(" ", "");

        //	for (int i = 0; i < curCnt; i++)
        //	{
        //		if (infoStorage[i].Name.Replace(" ", "").CompareTo(name) == 0)
        //		{
        //			return i;
        //		}
        //	}

        //	return -1;
        //}
        #endregion
        #endregion

        #region 배열과 HashSet SearchName오버라이드
        private bool SearchName(string name)
		{
            foreach(PhoneInfo info in infoStorage)
			{
                if (info.Name.Equals(name))
                    return true;
			}
            return false;
		}

        //private int SearchName(string name)
        // {
        //  for (int i = 0; i < curCnt; i++)
        // {
        // if (infoStorage[i].Name.Replace(" ", "").CompareTo(name) == 0)
        // {
        //  return i;
        //    }
        //     }

        //    return -1;
        //   }
        #endregion

        #region 배열과 HashSet DeleteData

        public void DeleteData()
        {
            Console.WriteLine("주소록 삭제를 시작합니다......");

            PhoneInfo delInfo = SearchName();
            if(delInfo == null)
			{
				Console.WriteLine("삭제할 데이터가 없습니다.");
			}
            else
			{
                infoStorage.Remove(delInfo);
			}
        }
		#region 배열의 DeleteData
		//public void DeleteData()
		//{
		//    Console.WriteLine("주소록 삭제를 시작합니다......");

		//    int dataIdx = SearchName();
		//    if (dataIdx < 0)
		//    {
		//        Console.WriteLine("삭제할 데이터가 없습니다");
		//    }
		//    else
		//    {
		//        for(int i=dataIdx; i<curCnt; i++)
		//        {
		//            infoStorage[i] = infoStorage[i + 1];
		//        }
		//        curCnt--;
		//        Console.WriteLine("주소록 삭제가 완료되었습니다");
		//    }
		//}
		#endregion
		#endregion
	}
}
