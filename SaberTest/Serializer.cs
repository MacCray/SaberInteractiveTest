using System.Text;
using System.Text.RegularExpressions;

namespace SaberTest
{
    public static class Serializer
    {
        /// <summary>
        /// Сериализация двусвязного списка.
        /// </summary>
        /// <param name="s">Поток для сохранения сериализованного списка.</param>
        /// <param name="list">Список для сериализации.</param>
        public static void Serialize(Stream s, ListRandom list)
        {
            string listJSON = ListToJson(list);
            byte[] byteArray = Encoding.UTF8.GetBytes(listJSON);
            s.Write(byteArray);
        }

        /// <summary>
        /// Десериализация двусвязного списка.
        /// </summary>
        /// <param name="s">Поток из которого читаются данные.</param>
        /// <param name="list">Список в который сохранаются результаты десериализации.</param>
        public static void Deserialize(Stream s, ListRandom list)
        {
            byte[] byteArray;
            if (s is MemoryStream)
                byteArray = (s as MemoryStream).ToArray();
            else
            {
                byteArray = new byte[s.Length];
                s.Read(byteArray);
            }
            var listJson = Encoding.UTF8.GetString(byteArray);
            JsonToList(listJson, list);
        }

        /// <summary>
        /// Преобразует исходный список в строку формата Json.
        /// </summary>
        /// <param name="list">Список для преобразования.</param>
        /// <returns>Строка формата Json.</returns>
        /// <exception cref="Exception"></exception>
        public static string ListToJson(ListRandom list)
        {
            Dictionary<ListNode, int> nodeIndexes = NodeIndexes(list);
            StringBuilder sb = new();
            sb.Append($"[{Environment.NewLine}");
            var node = list.Head;

            do
            {
                sb.Append($"\t{{{Environment.NewLine}");
                sb.Append($"\t\t\"Previous\": {(node.Previous != null ? nodeIndexes[node.Previous]: "null")},{Environment.NewLine}");
                sb.Append($"\t\t\"Next\": {(node.Next != null ? nodeIndexes[node.Next] : "null")},{Environment.NewLine}");
                sb.Append($"\t\t\"Random\": {(node.Random != null ? nodeIndexes[node.Random] : "null")},{Environment.NewLine}");
                sb.Append($"\t\t\"Data\": {(node.Data != null ? $"\"{node.Data}\"": "null")}{Environment.NewLine}");
                sb.Append($"\t}}");
                node = node.Next;
                if (node != null) sb.Append($",{Environment.NewLine}");
            }
            while (node != null);

            sb.Append($"{Environment.NewLine}]");
            return sb.ToString();
        }

        /// <summary>
        /// Формирование списка на основе строки формата Json.
        /// </summary>
        /// <param name="listJSON">Исходная строка в формате Json.</param>
        /// <param name="list">Список в который сохранаются результаты.</param>
        /// <exception cref="Exception"></exception>
        public static void JsonToList(string listJSON, ListRandom list)
        {
            Regex regex = new(@"(?=:).*");
            var NodesData = regex.Matches(listJSON);
            if (NodesData.Count < 4) throw new Exception("Строка Json не содержит необходимых данных.");
            List<ListNode> nodes = new(NodesData.Count / 4);

            for (int i = 0; i < NodesData.Count / 4; i++)
            {
                nodes.Add(new ListNode());
            }

            for (int i = 0; i < NodesData.Count; i += 4)
            {
                if (int.TryParse(NodesData[i].Value.AsSpan(2, NodesData[i].Value.Length - 4), out int prev))
                    nodes[i / 4].Previous = nodes[prev];
                if (int.TryParse(NodesData[i + 1].Value.AsSpan(2, NodesData[i + 1].Value.Length - 4), out int next))
                    nodes[i / 4].Next = nodes[next];
                if (int.TryParse(NodesData[i + 2].Value.AsSpan(2, NodesData[i + 2].Value.Length - 4), out int rand))
                    nodes[i / 4].Random = nodes[rand];
                nodes[i / 4].Data = NodesData[i + 3].Value[2..^1] != "null" ? NodesData[i + 3].Value[3..^2] : null;
            }

            list.Head = nodes.First();
            list.Tail = nodes.Last();
        }

        /// <summary>
        /// Создаёт Dictionary, в котором каждый ListNode связан со своим индексом внутри списка.
        /// </summary>
        /// <param name="list">Исходный список.</param>
        /// <returns>Dictionary c ListNode и их соответствующими индексами.</returns>
        static Dictionary<ListNode, int> NodeIndexes(ListRandom list)
        {
            Dictionary<ListNode, int> nodeIndexes = new();
            var node = list.Head;
            int i = 0;
            do
            {
                nodeIndexes.Add(node, i++);
                node = node.Next;
            }
            while (node != null);
            return nodeIndexes;
        }
    }
}
