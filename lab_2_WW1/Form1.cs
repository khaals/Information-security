using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;

namespace lab_2_WW1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private Dictionary<string, string> ADFGXTable = new Dictionary<string, string>()
        {
            {"F", "AA" },
            {"N", "AD" },
            {"H", "AF" },
            {"E", "AG" },
            {"Q", "AX" },
            {"R", "DA" },
            {"D", "DD" },
            {"Z", "DF" },
            {"O", "DG" },
            {"C", "DX" },
            {"I", "FA" },
            {"J", "FA" },
            {"S", "FD" },
            {"A", "FF" },
            {"G", "FG" },
            {"U", "FX" },
            {"B", "GA" },
            {"V", "GD" },
            {"K", "GF" },
            {"P", "GG" },
            {"W", "GX" },
            {"X", "XA" },
            {"M", "XD" },
            {"Y", "XF" },
            {"T", "XG" },
            {"L", "XX" }
        };

        // Замена символов на код согласно таблице (F это AA, C это DX и т.д.)
        private string ReplaceByTable(string msg)
        {
            string encoded = "";
            for (int j = 0; j < msg.Length; j++)
                if (ADFGXTable.ContainsKey(char.ToUpper(msg[j]).ToString()))
                    encoded += ADFGXTable[char.ToUpper(msg[j]).ToString()];
            return encoded;
        }

        // Функция создает таблицу из символов согласно длине ключа
        private List<List<string>> MakeTableByKey(string replacedMsg, string key)
        {
            List<List<string>> newTable = new List<List<string>>();
            int msgIndex = 0;
            int rowIndex = 0;
            while (msgIndex < replacedMsg.Length)
            {
                newTable.Add(new List<string>());
                // Заполняем таблицу
                for (int keyIndex = 0; keyIndex < key.Length; keyIndex++)
                {
                    if (msgIndex < replacedMsg.Length)
                        newTable[rowIndex].Add(replacedMsg[msgIndex].ToString());
                    else
                        newTable[rowIndex].Add("");
                    msgIndex++;
                }
                rowIndex++;
            }
            return newTable;
        }

        private string[,] MakeTableByKeyDecrypt(string replacedMsg, string key)
        {
            int msgIndex = 0;
            int height = (int)Math.Ceiling((decimal) replacedMsg.Length / key.Length);
            string[,] symbolsMatrix = new string[height, key.Length];
            int colIndex = 0;
            int gap = key.Length * height - replacedMsg.Length;
            var indexes = GetKeyIndexesAlphabeticalDecrypt(key);
            while (msgIndex < replacedMsg.Length)
            {
                for (int keyIndex = 0; keyIndex < height; keyIndex++)
                {
                    if (keyIndex == height - 1 && gap > 0 && key.Length - indexes[colIndex] <= gap)
                    {
                        symbolsMatrix[keyIndex, colIndex] = "";
                    } else
                    {
                        symbolsMatrix[keyIndex, colIndex] = replacedMsg[msgIndex] + "";
                        msgIndex++;
                    }
                }
                colIndex++;
            }
            return symbolsMatrix;
        }

        // Возвращает массив индексов букв в key согласно алфавитному порядку
        // Из Battle вернет [1, 0, 5, 4, 2, 3]
        private List<int> GetKeyIndexesAlphabetical(string key)
        {
            List<int> indexesAlphabetical = new List<int>();
            char[] origChars = key.ToCharArray();
            char[] sortedChars = key.ToCharArray();
            Array.Sort(sortedChars);
            for (int i = 0; i < sortedChars.Length; i++)
            {
                int keyLetterIndex = Array.IndexOf(origChars, sortedChars[i]);
                indexesAlphabetical.Add(keyLetterIndex);
                origChars[keyLetterIndex] = ' ';
            }
            return indexesAlphabetical;
        }

        // Возвращает массив индексов букв key в оригинальном ключе относительно сортированного по алфавиту
        // Из Battle вернет [1, 0, 4, 5, 3, 2]
        private List<int> GetKeyIndexesAlphabeticalDecrypt(string key)
        {
            List<int> indexesAlphabetical = new List<int>();
            char[] origChars = key.ToCharArray();
            char[] sortedChars = key.ToCharArray();
            Array.Sort(sortedChars);
            for (int i = 0; i < origChars.Length; i++)
            {
                int keyLetterIndex = Array.IndexOf(sortedChars, origChars[i]);
                indexesAlphabetical.Add(keyLetterIndex);
                sortedChars[keyLetterIndex] = ' ';
            }
            return indexesAlphabetical;
        }

        // Шифрование сообщения
        private string Encode(string replacedMsg, string key)
        {
            string finalMsg = "";
            List<List<string>> symbolsTable = MakeTableByKey(replacedMsg, key);
            List<int> alphabeticalIndexes = GetKeyIndexesAlphabetical(key);
            for (int j = 0; j < symbolsTable[0].Count; j++)
            {
                for (int i = 0; i < symbolsTable.Count; i++)
                {
                    finalMsg += symbolsTable[i][alphabeticalIndexes[j]];
                }
            }
            return finalMsg;
        }

        static List<List<string>> TransposeMatrix(List<List<string>> matrix)
        {
            int rows = matrix.Count;
            int cols = matrix[0].Count;

            // Создаем новый список списков строк с переставленными размерами
            List<List<string>> transposedMatrix = new List<List<string>>();

            // Инициализируем транспонированную матрицу
            for (int j = 0; j < cols; j++)
            {
                transposedMatrix.Add(new List<string>());
            }

            // Заполняем транспонированную матрицу
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    transposedMatrix[j].Add(matrix[i][j]);
                }
            }

            return transposedMatrix;
        }

        private string Decode(string[,] table, string key)
        {
            string decodedMsg = "";
            string decodedCode = "";
            var indexes = GetKeyIndexesAlphabeticalDecrypt(key);
            int rows = table.GetUpperBound(0) + 1;    // количество строк
            int columns = table.Length / rows;
            for (int i = 0; i< rows; i++)
            {
                for (int j = 0; j< columns; j++)
                {
                    decodedCode += table[i, indexes[j]];
                }
            }
            for (int i = 0; i< decodedCode.Length; i+=2)
            {
                string newPair = string.Concat(decodedCode[i], decodedCode[i + 1]);
                string foundKey = ADFGXTable.FirstOrDefault(x => x.Value == newPair).Key;
                decodedMsg += foundKey;
            }

            return decodedMsg;
        }


        // Шифр сообщения
        private void button1_Click(object sender, EventArgs e)
        {
            string msg = textBox1.Text;
            string key = textBox4.Text.ToUpper();
            string replacedMsg = ReplaceByTable(msg);
            textBox8.Text = Encode(replacedMsg, key);
        }


        // Дешифр сообщения
        private void button2_Click(object sender, EventArgs e)
        {
            string msg = textBox2.Text;
            string key = textBox3.Text.ToUpper();
            var table = MakeTableByKeyDecrypt(msg, key);
            //var transposedTable = TransposeMatrix(table);
            textBox9.Text = Decode(table, key);
        }
    }
}
