using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VladosProjectV2
{
    internal class Status_Calculate
    {
        private static ProjectDB dbContext = new ProjectDB();


        protected static int progress_value;

        public static int CalculateProgress(int clientID, bool status)
        {
            progress_value = 0;
            List<string> Names = dbContext.Documents.AsEnumerable().Where(p => p.Client_ID == clientID
            && p.Status == true).Select(p => p.Document_name).ToList();
            int countdocs = dbContext.Documents.AsEnumerable().Where(p => p.Client_ID == clientID && p.Status == status).Count();

            for (int i = 0; i < countdocs; i++)
            {
                if (Names[i] == "Вид на жительство" || Names[i] == "Временная регистрация" ||
                    Names[i] == "Разрешение на временное проживание" || Names[i] == "Результат медицинской комиссии" ||
                    Names[i] == "Трудовой патент" || Names[i] == "Сертификат о знании русского языка")
                {
                    progress_value += 5;
                }

                else { progress_value += 2; }
            }

            return progress_value;
        }
    }
}
