namespace SaberTest
{
    public class ListRandom
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;

        public void Serialize(Stream s)
        {
            if (Head == null)
                throw new Exception("Исходный список не содержит элементов");
            Serializer.Serialize(s, this);
        }

        public void Deserialize(Stream s)
        {
            if (s.Length == 0)
                throw new Exception("Поток не содержит данных");
            Serializer.Deserialize(s, this);
        }
    }
}
