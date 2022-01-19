using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork7._8.v2
{
    class Employee
    {
        /// <summary>
        /// Создание нового сотрудника на основе считанной записи из файла базы
        /// </summary>
        /// <param name="databaseEntry">считанная из файла строка</param>
        public Employee(string databaseEntry)
        {
            string[] entryValues = databaseEntry.Split('#');

            this.Id = Convert.ToInt32(entryValues[0]);
            this.LastModified = Convert.ToDateTime(entryValues[1]);
            this.Name = entryValues[2];
            this.Height = Convert.ToInt32(entryValues[4]);
            this.BirthDay = Convert.ToDateTime(entryValues[5]);
            this.BirthPlace = entryValues[6];
        }

        /// <summary>
        /// Создание нового сотрудника
        /// </summary>
        /// <param name="id">Порядковый номер в базе. Высчитывается по последней записи на момент создания +1</param>
        /// <param name="fullName">Ф.И.О сотрудника</param>
        /// <param name="heigh">Рост сотрудника</param>
        /// <param name="bDay">День рождения сотрудника</param>
        /// <param name="bPlace">Место рождения сотрудника</param>
        /// <param name="fileString">Строчка в файле базы</param>
        public Employee() { }
        public Employee(string fullName, int height, DateTime bDay, string bPlace)
        {
            
            this.LastModified = DateTime.Now;
            this.Name = fullName;
            this.Height = height;
            this.BirthDay = bDay;
            this.BirthPlace = bPlace;
        }

        public Employee(int id, string fullName, int heigh, DateTime bDay, string bPlace)
        {
            this.Id = id;
            this.LastModified = DateTime.Now;
            this.Name = fullName;
            this.Height = heigh;
            this.BirthDay = bDay;
            this.BirthPlace = bPlace;
        }
        
        #region Объявление автосвойств
        public int Id { get; set; }
        /// <summary>
        /// Дата создания записи в базе
        /// </summary>
        public DateTime LastModified { get; set; }
        /// <summary>
        /// Ф.И.О сотрудника
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Возраст сотрудника
        /// </summary>
        public int Age()
        {
            return Convert.ToInt32(Math.Round(((DateTime.Now - BirthDay).TotalDays / 365.25), 0));

        }
        /// <summary>
        /// Рост сотрудника
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// Дата рождения сотрудника
        /// </summary>
        public DateTime BirthDay { get; set; }
        /// <summary>
        /// Место рождения сотрудника
        /// </summary>
        public string BirthPlace { get; set; }
        #endregion
    }
}
