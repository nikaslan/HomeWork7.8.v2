using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HomeWork7._8.v2
{
    class EmployeeRepository
    {
        #region Объявление перменных и инициализация

        ApplicationContext db; // создали объект ссылающийся на базу данных
        
        /// <summary>
        /// Массив переменных типа Employee
        /// </summary>
        private Employee[] employees;       
        
        /// <summary>
        /// Размер базы данных (количество строчек в файле)
        /// </summary>
        private int databaseSize;
        private string titles;

        /// <summary>
        /// Создание временной рабочей копии базы данных
        /// </summary>
        /// <param name="filePath"></param>
        public EmployeeRepository()
        {
            db = new ApplicationContext();
            this.databaseSize = db.Employees.Count(); // определяем размер массива путем считывания размера таблицы Employees
            this.employees = new Employee[databaseSize];
            this.titles = "ID | Добавлен        | Имя                      | Возраст | Рост | Дата рож. | Место рождения";
            LoadDatabase();
        }
        #endregion

        #region Загрузка и печать данных

        /// <summary>
        /// считывание данных из базы во временную рабочую копию базы
        /// </summary>
        public void LoadDatabase()
        {

            // читаем базу во временный список, затем из списка каждый элемент записываем в массив Employee[]

            int i = 0;
            List<Employee> employees = db.Employees.ToList();

            foreach (Employee employee in employees)
            {
                this.employees[i] = employee;
                i++;
            }            
        }
        /// <summary>
        /// Вывод содержимого репозитория в консоль
        /// </summary>
        public void PrintDatabaseToConsole()
        {
            Console.WriteLine("Список сотрудников:");
            Console.WriteLine($"\n{titles}");

            bool isEmpty = true; // проверяем существуют ли записи в репозитории. Записей не будет если файл был пуст или был только что создан

            foreach (Employee employee in this.employees)
            {
                PrintEmployee(employee);
                isEmpty = false;
            }
            if (isEmpty)
            {
                Console.WriteLine("База сотрудников пуста");
            }

        }

        /// <summary>
        /// вывод информации о сотруднике в консоль
        /// </summary>
        public void PrintEmployee(Employee employee)
        {
            Console.WriteLine($"{employee.Id,3}| {employee.LastModified,16:dd.MM.yyyy HH:mm}| {employee.Name,25}| {employee.Age(),8}| {employee.Height,3}см| {employee.BirthDay,10:dd.MM.yyyy}| {employee.BirthPlace,20}");
        }
        #endregion

        #region Добавление и изменение записей
        /// <summary>
        /// Запись нового значения Employee в файл
        /// ЭТОТ МЕТОД Я ХОЧУ ИСПОЛЬЗОВАТЬ ДЛЯ ЗАПИСИ КАК НОВОЙ СТРОКИ В ФАЙЛ, ТАК И ДЛЯ ПЕРЕЗАПИСИ УЖЕ СУЩЕСТВУЮЩЕЙ
        /// </summary>
        /// <param name="newEntry">экземпляр Employee с новыми данными</param>
        /// <param name="line">Порядковый номер элемента в массиве, который мы будем записывать. -1 для создания новой записи</param>
        private void UpdateDatabaseEntry(Employee newEntry, int line)
        {
            Console.WriteLine("\nВ справочник будет внесена следующая запись:");
            Console.WriteLine($"\n{titles}");
            PrintEmployee(newEntry);

            Console.WriteLine("\nЕсли хотите подтвердить внесение записи, нажмите Enter. Для отмены - нажмите Esc.");
            while (true)
            {

                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.Enter)
                {
                    if (line == -1) // -1 передаем когда добавляем новую запись
                    {
                        databaseSize++;
                        Array.Resize(ref employees, databaseSize);
                        employees[databaseSize - 1] = newEntry;
                        db.Employees.Add(newEntry);
                    }
                    else
                    {
                        employees[line] = newEntry; // перезапись элемента массива под номером = line новыми данными
                        var query =
                            from emp in db.Employees
                            where emp.Id == newEntry.Id
                            select emp;

                        foreach(Employee emp in query)
                        {
                            emp.Name = newEntry.Name;
                            emp.Height = newEntry.Height;
                            emp.BirthDay = newEntry.BirthDay;
                            emp.BirthPlace = newEntry.BirthPlace;
                        }
                        //db.SaveChanges();
                    }
                    db.SaveChanges();
                    Console.WriteLine("\nЗапись внесена");
                    break;
                }
                else if (key == ConsoleKey.Escape)
                {
                    break;
                } // выходим без добавления изменений
                // если нажали на что-то, кроме Enter или Esc, то это не будет принято 
            }
        }
        /// <summary>
        /// Диалог добавления записи нового сотрудника
        /// </summary>
        /// <returns></returns>
        private Employee NewEmployee()
        {
            Employee newEmployee;

            // ввод имени
            Console.WriteLine("Введите имя нового сотрудника. *Только первые 25 символов имени будут сохранены:");
            string newName = Console.ReadLine();
            if (newName.Length > 25) newName = newName.Substring(0, 25);

            // ввод роста
            Console.WriteLine("\nВведите рост нового сотрудника в сантимертах:");
            int newHeigh;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out newHeigh))
                {
                    if (newHeigh < 50 || newHeigh > 250)
                    {
                        Console.WriteLine("Введенный рост должен быть между 50 см и 250 см");
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Некорректный данные о росте.");
                }

            }

            // ввод дня рождения
            Console.WriteLine("\nВведите дату рождения нового сотрудника в формате дд.мм.гггг:");
            DateTime newBirthDate;
            while (true)
            {

                if (DateTime.TryParse(Console.ReadLine(), out newBirthDate))
                {

                    if (newBirthDate > DateTime.Now)
                    {
                        Console.WriteLine("Дата рождения должна быть в прошлом.");
                        continue;
                    }
                    break;
                }
                else
                {
                    Console.WriteLine("Некорректный формат времени.");
                }

            }

            // Ввод города рождения
            Console.WriteLine("\nВведите место рождения нового сотрудника:");
            string newBirthPlace = Console.ReadLine();
            if (newBirthPlace.Length > 20) newBirthPlace = newBirthPlace.Substring(0, 20);
            
            newEmployee = new Employee(newName, newHeigh, newBirthDate, newBirthPlace);

            return newEmployee;
        }
        /// <summary>
        /// Диалог изменения записи сотрудник
        /// </summary>
        /// <param name="line">Передаем порядковый номер записи, которую нужно изменить, в массиве employees</param>
        /// <returns></returns>
        private Employee EditEmployee(int line)
        {
            // создаем переменные, в которых мы будем сохранять измененные данные. При создании данные совпадают с оригиналом
            string editName = employees[line].Name;
            int editHeigh = employees[line].Height;
            DateTime editBirthDate = employees[line].BirthDay;
            string editBirthPlace = employees[line].BirthPlace;

            Console.WriteLine("Укажите какое значение в записи вы хотите изменить:");
            Console.WriteLine("Нажмите 1 для изменения Ф.И.О сотрудника");
            Console.WriteLine("Нажмите 2 для изменения информации о росте сотрудника");
            Console.WriteLine("Нажмите 3 для изменения информации о дате рождения сотрудника");
            Console.WriteLine("Нажмите 4 для изменения информации о месте рождения сотрудника");

            while (true)
            {
                var key = Console.ReadKey(true).Key;
                // меняем ФИО
                if (((char)key) == '1')
                {
                    Console.WriteLine("Введите новое значение Ф.И.О и нажмите Enter. *Только первые 25 символов имени будут сохранены:");
                    editName = Console.ReadLine();
                    if (editName.Length > 25) editName = editName.Substring(0, 25);
                    break;
                }
                // меняем рост
                else if (((char)key) == '2')
                {
                    Console.WriteLine("\nВведите новое значение роста сотрудника в сантимертах:");

                    while (true)
                    {
                        if (int.TryParse(Console.ReadLine(), out editHeigh))
                        {
                            if (editHeigh < 50 || editHeigh > 250)
                            {
                                Console.WriteLine("Введенный рост должен быть между 50 см и 250 см");
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }
                        else { Console.WriteLine("Некорректный данные о росте."); }
                    }
                    break;
                }
                // меняем дату рождения
                else if (((char)key) == '3')
                {
                    Console.WriteLine("\nВведите новое значение даты рождения сотрудника в формате дд.мм.гггг:");

                    while (true)
                    {
                        if (DateTime.TryParse(Console.ReadLine(), out editBirthDate))
                        {

                            if (editBirthDate > DateTime.Now)
                            {
                                Console.WriteLine("Дата рождения должна быть в прошлом.");
                                continue;
                            }
                            break;
                        }
                        else { Console.WriteLine("Некорректный формат времени."); }
                    }
                    break;
                }
                // меняем место рождения
                else if (((char)key) == '4')
                {
                    Console.WriteLine("\nВведите новое значение места рождения сотрудника:");
                    editBirthPlace = Console.ReadLine();
                    if (editBirthPlace.Length > 20) editBirthPlace = editBirthPlace.Substring(0, 20);
                    break;
                }
            }

            Employee editEmployee = new Employee(employees[line].Id, editName, editHeigh, editBirthDate, editBirthPlace);

            return editEmployee;
        }
        /// <summary>
        /// Метод добавления нового сотрудника, который можно вызывать извне 
        /// </summary>
        public void AddEmployee()
        {
            UpdateDatabaseEntry(NewEmployee(), -1);
        }
        /// <summary>
        /// Метод просмотра и изменения сотрудника, который можно вызывать извне
        /// </summary>
        public void ViewAndEditEmployee()
        {
            bool employeeFound = false;
            int tempNum = 0; // порядковый номер нужной нам записи в репозитории


            while (true)
            {
                Console.WriteLine("Для просмотра сведений о сотруднике введите его ID.");
                if (Int32.TryParse(Console.ReadLine(), out int employeeID))
                {
                    foreach (Employee employee in employees) // сравниваем введенный ID со всеми ID сотрудников, которых считали в репозиторий
                    {
                        if (employeeID == employee.Id)
                        {
                            employeeFound = true; // нашли совпадение
                            break;
                        }
                        tempNum++;
                    }

                    if (employeeFound) // если мы нашли сотрудника с нужным ID
                    {
                        Console.Clear();
                        Console.WriteLine($"\n{titles}");
                        PrintEmployee(employees[tempNum]);
                        break;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Пользователя с таким ID нет в базе.");
                    }
                }
                Console.WriteLine("\nВведен некорректный ID.");
                Console.WriteLine("Нажмите Esc для выхода. Нажмите любую клавишу, чтобы ввести другой ID.");
                var keyPressed = Console.ReadKey(true);
                if (keyPressed.Key == ConsoleKey.Escape) { return; } // выходим если пользователь нажал Esc
                Console.Clear();
            }

            Console.WriteLine("\nЕсли хотите внести изменения, нажмите Enter. Для возврата в предыдущее меню - нажмите Esc.");
            while (true)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.Enter)
                {
                    UpdateDatabaseEntry(EditEmployee(tempNum), tempNum);
                    break;
                }
                else if (key == ConsoleKey.Escape)
                {
                    break;
                }

            }
        }

        #endregion

        #region Дополнительные функции вывода в консоль
        /// <summary>
        /// Вывод записей, сделанных в определенном диапазоне дат
        /// </summary>
        public void PrintDateRange()
        {
            DateTime startDate; // дата начала диапазона
            DateTime endDate; // дата конца диапазона

            Console.WriteLine("Введите дату начала диапазона");
            while (true)
            {

                if (DateTime.TryParse(Console.ReadLine(), out startDate))
                {

                    if (startDate > DateTime.Now)
                    {
                        Console.WriteLine("Дата начала диапазона должна быть в прошлом.");
                        continue;
                    }
                    break;
                }
                else
                {
                    Console.WriteLine("Некорректный формат времени.");
                }

            }
            Console.WriteLine("Введите дату конца диапазона");
            while (true)
            {

                if (DateTime.TryParse(Console.ReadLine(), out endDate))
                {

                    if (startDate > endDate)
                    {
                        Console.WriteLine("Дата конца диапазона не может быть раньше, чем дата начала диапазона");
                        continue;
                    }
                    break;
                }
                else
                {
                    Console.WriteLine("Некорректный формат времени.");
                }

            }

            /// я думал рализовать ли тут сортировку записей перед выводом
            /// Но решил что сделаю два разных подхода. Т.к. сортировку я буду реализовывать в специальном методе
            /// то в данном методе сделаю упрощенную версию, без сортировки
            Console.WriteLine("\nНайденные записи:");
            Console.WriteLine($"\n{titles}");

            foreach (Employee employee in employees)
            {
                if (employee.LastModified >= startDate && employee.LastModified <= endDate)
                {
                    PrintEmployee(employee);
                }
            }

        }

        /// <summary>
        /// Вывод записей в отсортированном виде
        /// </summary>
        public void PrintSorted()
        {
            // запрашиваю по какому критерию пользователь хочет увидеть сортировку. Для практики запрашиваю поля с разными типами данных. Остальные будут реализовываться так же
            Console.WriteLine("Пожалуйста выберите по какому критерию хотите вывести отсортированные данные:");
            Console.WriteLine("Нажмите 1 для сортировки по дате создания записей"); // сортировка даты. Так же сортировались бы данные по дате рождения
            Console.WriteLine("Нажмите 2 для сортировки по Ф.И.О сотрудников"); // сортировка строки. Так же сортировались бы данные по месту рождения
            Console.WriteLine("Нажмите 3 для сортировки по росту сотрудников"); // сортировка инт. Так же сортировались бы данные по возрасту

            // сортируем массив данных, записывая во пременный массив
            while (true)
            {
                var key = Console.ReadKey(true).Key;
                if (((char)key) == '1')
                {
                    Console.Clear();

                    Console.ReadKey(true);
                    Console.Clear();
                    continue;
                }
                else if (((char)key) == '2')
                {
                    Console.Clear();

                    Console.Clear();
                    continue;
                }
                else if (((char)key) == '3')
                {
                    Console.Clear();

                    Console.ReadKey(true);
                    Console.Clear();
                    continue;
                }
            }
            // выводим в консоль получившийся временный массив
        }

        #endregion
    }
}
