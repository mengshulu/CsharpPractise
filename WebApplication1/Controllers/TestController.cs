using System; // Date 等等函示使用
using System.Collections.Generic; // List<> 類別使用
using System.Linq;
using System.Web;
using System.Web.Mvc; // Controller 使用

namespace WebApplication1.Controllers
{
    public class TestController : Controller
    {
        public class Student
        {
            public string id { get; set; }
            public string name { get; set; }
            public int score { get; set; }
            public Student()
            {
                id = string.Empty;
                name = string.Empty;
                score = 0;
            }
            public Student(string _id, string _name, int _score)
            {
                id = _id;
                name = _name;
                score = _score;
            }
            // 在C#當中所有的物件都可以使用ToString()方法,會將專案路徑寫入,所以重新定義
            public override string ToString()
            {
                return $"學號:{id}, 姓名:{name}, 分數:{score}.";
            }
        }
        
        public ActionResult Index()
        {
            DateTime date = DateTime.Now;
            Student data = new Student();
            List<Student> list = new List<Student>();
            // List是C#預設的一個陣列，可以放任何物件
            list.Add(new Student("1", "小明", 80));
            list.Add(new Student("2", "小華", 70));
            list.Add(new Student("3", "小英", 60));
            list.Add(new Student("4", "小李", 50));
            list.Add(new Student("5", "小張", 90));

            ViewBag.Date = date;
            ViewBag.Student = data;
            ViewBag.List = list;
            return View();
        }

        public ActionResult CreateStudent()
        {
            DateTime date = DateTime.Now;
            ViewBag.Date = date;

            Student data = new Student("1", "小明", 80);
            return View(data);
        }
        
        public ActionResult Transcripts(string id, string name, int score)
        {
            Student data = new Student(id, name, score);
            return View(data);
        }
        
        [HttpPost]
        public ActionResult Transcripts(FormCollection post)
        {
            string id = post["id"];
            string name = post["name"];
            int score = Convert.ToInt32(post["score"]);
            Student data = new Student(id, name, score);
            return View(data);
        }
    }
}