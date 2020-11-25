using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kindergarten.Models;
using Microsoft.AspNetCore.Razor.Language;

namespace Kindergarten.Data
{
    public static class DbInitialize
    {
        private static Random random = new Random();

        private static string[] namesM =
        {
            "Екатерина", "Виолетта", "Наталья", "Анна", "Алина", "Евгения", "Александра", "Любовь", "Татьяна",
            "Вероника", "Ольга", "Светлана", "Елена", "Надежда", "Юлия"
        };

        private static string[] surnamesM =
        {
            "Кириленко", "Панфилова", "Дубровская", "Долгова", "Яркина", "Черкасова", "Рябцева", "Жилина", "Фокина",
            "Мосина", "Агапова", "Якубовская", "Николина", "Котова", "Прохорова"
        };

        private static string[] middleNamesM =
        {
            "Алексеевна", "Дмитриевна", "Владимировна", "Евгеньевна", "Артёмовна", "Павловна", "Николаевна",
            "Антоновна", "Викторовна", "Кирилловна", "Андреевна", "Геннадьевна", "Данииловна", "Олеговна", "Егоровна"
        };

        private static string[] namesF =
        {
            "Даниил", "Александр", "Алексей", "Кирилл", "Дмитрий", "Олег", "Евгений", "Игорь", "Павел", "Денис",
            "Николай", "Егор", "Антон", "Максим", "Геннадий", "Святослав", "Сергей", "Кузьма", "Владислав", "Иван"
        };

        private static string[] surnamesF =
        {
            "Третьяков", "Медведев", "Станченко", "Кищук", "Бутрим", "Михальчук", "Бращук", "Ульяненко", "Черняков",
            "Серединский", "Вересковский", "Киркоров", "Покровский", "Чуприс", "Магомедов", "Могилевец", "Каврига",
            "Романко", "Маценко", "Морозов"
        };

        private static string[] middleNamesF =
        {
            "Николаевич", "Владимирович", "Иванович", "Евгеньевич", "Кириллович", "Андреевич", "Алексеевич",
            "Дмитриевич", "Васильевич", "Георгиевич", "Даниилович", "Артемьевич", "Артурович", "Олегович", "Максимович",
            "Романович", "Александрович", "Павлович", "Кузьмич", "Владиславович"
        };

        private static string[] adresses = { "бульвар Юбилейный", "проспект Машерова", "улица Ленина", "улица Королева", "бульвар Днепровский", "улица Рокоссовского", "улица Горовца", "улица Ванеева", "улица Долгобродская", "улица Козлова", "улица Голодеда", "проспект Мира", "проспект Независимисти", "улица Народного Ополчения", "улица Миронова", "улица Веры Хоружей", "улица Нёманская", "улица Мояковского", "Партизанский проспект", "улица Ангарская" };

        private static string[] cities =
        {
            "Минск", "Могилев", "Барановичи", "Гомель", "Чаусы", "Борисов", "Витебск", "Гродно", "Жлобин", "Москва",
            "Киев", "Полоцк", "Речица", "Любань", "Слуцк"
        };

        private static string[] groupTypes =
        {
            "Младшая группа", "Старшая группа", "Средняя группа", "Подготовительная группа", "Ясли"
        };

        private static string[] otherGroups =
        {
            "Рисование", "Хореография", "Пение", "Вышивание", "Танцы", "Актерское мастерство", "Плетение", "Гончарство"
        };

        private static string[] positions =
        {
            "Заведующий", "Заместитель директора по учебно-воспитательной работе", "Старший воспитатель",
            "Заведующий по административно-хозяйственной части", "Сторож", "Медицинский работник", "Учитель-логопед",
            "Инструктор по физической культуре", "Музыкальный руководитель", "Педагог-психолог", "Повар",
            "Помощник воспитателя", "Кухонный работник", "Обслуживающий персонал", "Дворник"
        };

        private static char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();
        private static string[] phoneCodes = {"33", "29", "44", "17", "25"};

        private static string GetRandomEl(string[] arr)
        {
            return arr[random.Next(0, arr.Length)];
        }

        private static DateTime NextDateTime()
        {
            DateTime start = new DateTime(2015, 1, 1);
            int range = (DateTime.Today - start).Days;

            return start.AddDays(random.Next(range));
        }
        private static string GetString(int minStringLength, int maxStringLength)
        {
            string result = "";

            int stringLimit = minStringLength + random.Next(maxStringLength - minStringLength);

            int stringPosition;
            for (int i = 0; i < stringLimit; i++)
            {
                stringPosition = random.Next(letters.Length);

                result += letters[stringPosition];
            }

            return result;
        }

        public static void Initialize(kindergartenContext db)
        {
            db.Database.EnsureCreated();
            int rowCount;
            int rowIndex;

            if (!db.Positions.Any())
            {
                rowCount = positions.Length;
                rowIndex = 0;

                while (rowIndex < rowCount)
                {
                    Position position = new Position
                    {
                        PositionName = GetRandomEl(positions)
                    };

                    db.Positions.Add(position);
                    rowIndex++;
                }

                db.SaveChanges();
            }

            if (!db.GroupTypes.Any())
            {
                rowCount = groupTypes.Length;
                rowIndex = 0;

                while (rowIndex < rowCount)
                {
                    GroupType groupType = new GroupType
                    {
                        NameOfType = GetRandomEl(groupTypes),
                        Note = GetString(14, 20)
                    };
                    db.GroupTypes.Add(groupType);
                    rowIndex++;
                }

                db.SaveChanges();
            }

            if (!db.Staff.Any())
            {
                rowCount = 1000;
                rowIndex = 0;

                int minPosId = db.Positions.Min(x => x.Id);
                int maxPosId = db.Positions.Max(x => x.Id);
                while (rowIndex < rowCount)
                {
                    Staff staff = new Staff
                    {
                        FullName = GetRandomEl(surnamesM) + " " + GetRandomEl(namesM) + " " + GetRandomEl(middleNamesM),
                        Adress = GetRandomEl(cities),
                        Phone = Convert.ToInt32(GetRandomEl(phoneCodes) +
                                                Convert.ToString(random.Next(1000000, 9999999))),
                        PositionId = random.Next(minPosId, maxPosId + 1),
                        Info = GetString(10, 16),
                        Reward = GetString(8, 12)
                    };
                    db.Staff.Add(staff);
                    rowIndex++;
                }

                db.SaveChanges();
            }

            if (!db.Groups.Any())
            {
                rowIndex = 0;
                rowCount = 1000;

                int minStaffId = db.Staff.Min(x => x.Id);
                int maxStaffId = db.Staff.Max(x => x.Id);

                int minTypeId = db.GroupTypes.Min(x => x.Id);
                int maxTypeId = db.GroupTypes.Max(x => x.Id);

                while (rowIndex < rowCount)
                {
                    Group group = new Group
                    {
                        GroupName = GetString(1, 2) + Convert.ToString(random.Next(1, 4)),
                        StaffId = random.Next(minStaffId, maxStaffId + 1),
                        CountOfChildren = random.Next(20, 28),
                        YearOfCreation = random.Next(2015, 2020),
                        TypeId = random.Next(minTypeId, maxTypeId)
                    };
                    db.Groups.Add(group);
                    rowIndex++;
                }

                db.SaveChanges();
            }

            if (!db.Parents.Any())
            {
                rowIndex = 0;
                rowCount = 100;

                while (rowIndex < rowCount)
                {
                    Parent parent = new Parent
                    {
                        Mfullname = GetRandomEl(surnamesM) + " " + GetRandomEl(namesM) + " " + GetRandomEl(middleNamesM),
                        Ffullname = GetRandomEl(surnamesF) + " " + GetRandomEl(namesF) + " " + GetRandomEl(middleNamesF)
                    };
                    db.Parents.Add(parent);
                    rowIndex++;
                }

                db.SaveChanges();
            }

            if (!db.Children.Any())
            {
                rowIndex = 0;
                rowCount = 1000;

                while (rowIndex < rowCount)
                {

                    int gMinId = db.Groups.Min(x => x.Id);
                    int gMaxId = db.Groups.Max(x => x.Id);

                    int pMinId = db.Parents.Min(x => x.Id);
                    int pMaxId = db.Parents.Max(x => x.Id);

                    Child ch = new Child
                    {
                        FullName = random.Next(0,2) == 1 ? GetRandomEl(surnamesM) + " " + GetRandomEl(namesM) + " " + GetRandomEl(middleNamesM) : GetRandomEl(surnamesF) + " " + GetRandomEl(namesF) + " " + GetRandomEl(middleNamesF),
                        BirthDate = NextDateTime(),
                        Gender = random.Next(0, 2) == 1 ? "M" : "F",
                        ParentId = random.Next(pMinId, pMaxId+1),
                        Adress = GetRandomEl(adresses),
                        GroupId = random.Next(gMinId, gMaxId+1),
                        Note = GetString(6,8),
                        OtherGroup = GetRandomEl(otherGroups)
                    };
                    db.Children.Add(ch);
                    rowIndex++;
                }

                db.SaveChanges();
            }
        }
    }
}
