using SaberTest;

namespace NUnitTests
{
    public class SerializeTests
    {
        ListRandom list;

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void EmptyList_ThrowsException()
        {
            list = new();
            var ex = Assert.Throws<Exception>(() => list.Serialize(Stream.Null));
            Assert.That(ex.Message, Is.EqualTo("Исходный список не содержит элементов"));
        }

        [Test]
        public void List_2Nodes_CorrectJsonString()
        {
            ListNode node1 = new() { Data = "1" };
            ListNode node2 = new() { Previous = node1 };
            node1.Next = node2;
            node1.Random = node2;

            node2.Random = node2;

            list = new() { Head = node1, Tail = node2 };
            string expectedJsonString = "[\r\n\t{\r\n\t\t\"Previous\": null,\r\n\t\t\"Next\": 1,\r\n\t\t\"Random\": 1,\r\n\t\t\"Data\": \"1\"\r\n\t},\r\n\t{\r\n\t\t\"Previous\": 0,\r\n\t\t\"Next\": null,\r\n\t\t\"Random\": 1,\r\n\t\t\"Data\": null\r\n\t}\r\n]";
            Assert.That(Serializer.ListToJson(list), Is.EqualTo(expectedJsonString));
        }

        [Test]
        public void List_SingleNode_CorrectJsonString()
        {
            ListNode node1 = new();
            node1.Random = node1;

            list = new() { Head = node1, Tail = node1 };
            string expectedJsonString = "[\r\n\t{\r\n\t\t\"Previous\": null,\r\n\t\t\"Next\": null,\r\n\t\t\"Random\": 0,\r\n\t\t\"Data\": null\r\n\t}\r\n]";
            Assert.That(Serializer.ListToJson(list), Is.EqualTo(expectedJsonString));
        }

        [Test]
        public void List_MoreThan2Nodes_CorrectJsonString()
        {
            ListNode node1 = new() { Data = "1" };
            ListNode node2 = new() { Data = "2", Previous = node1 };
            ListNode node3 = new() { Data = "3", Previous = node2 };
            ListNode node4 = new() { Data = "4", Previous = node3 };
            ListNode node5 = new() { Data = "5", Previous = node4, Random = node2 };
            node1.Next = node2;
            node1.Random = node5;

            node2.Next = node3;
            node2.Random = node2;

            node3.Next = node4;
            node3.Random = node4;

            node4.Next = node5;
            node4.Random = node3;


            list = new() { Head = node1, Tail = node5 };
            string expectedJsonString = "[\r\n\t{\r\n\t\t\"Previous\": null,\r\n\t\t\"Next\": 1,\r\n\t\t\"Random\": 4,\r\n\t\t\"Data\": \"1\"\r\n\t},\r\n\t{\r\n\t\t\"Previous\": 0,\r\n\t\t\"Next\": 2,\r\n\t\t\"Random\": 1,\r\n\t\t\"Data\": \"2\"\r\n\t},\r\n\t{\r\n\t\t\"Previous\": 1,\r\n\t\t\"Next\": 3,\r\n\t\t\"Random\": 3,\r\n\t\t\"Data\": \"3\"\r\n\t},\r\n\t{\r\n\t\t\"Previous\": 2,\r\n\t\t\"Next\": 4,\r\n\t\t\"Random\": 2,\r\n\t\t\"Data\": \"4\"\r\n\t},\r\n\t{\r\n\t\t\"Previous\": 3,\r\n\t\t\"Next\": null,\r\n\t\t\"Random\": 1,\r\n\t\t\"Data\": \"5\"\r\n\t}\r\n]";
            Assert.That(Serializer.ListToJson(list), Is.EqualTo(expectedJsonString));
        }
    }
}