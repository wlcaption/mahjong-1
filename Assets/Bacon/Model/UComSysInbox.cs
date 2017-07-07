using Maria;
using Sproto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bacon.Model {
   public class UComSysInbox : Component {
        private Dictionary<long, Sysmail> _dic = new Dictionary<long, Sysmail>();
        private List<Sysmail> _li = new List<Sysmail>();

        public UComSysInbox(Entity entity) : base(entity) {
        }

        public Sysmail CreateMail() {
            return new Sysmail();
        }

        public void Add(Sysmail mail) {
            _dic[mail.Id] = mail;
            _li.Add(mail);
            _li.Sort();
        }

        public void Remove(Sysmail mail) {
            _dic.Remove(mail.Id);
            _li.Remove(mail);
        }

        public Sysmail GetMail(long id) {
            return _dic[id];
        }

        //IEnumerator IEnumerable.GetEnumerator() {
        //    for (int i = 0; i < _li.Count; i++) {
        //        yield return _li[i];
        //    }
        //}

        //IEnumerator<Sysmail> IEnumerable<Sysmail>.GetEnumerator() {
        //    for (int i = 0; i < _li.Count; i++) {
        //        yield return _li[i];
        //    }
        //}

        public int Count { get { return _li.Count; } }

        public void Clear() {
            _dic.Clear();
            _li.Clear();
        }

        public void Fetch() {

        }

        public void FetchSysmail(SprotoTypeBase responseObj) {
            //SysInbox sib = _service.SysInBox;
            //sib.Clear();
            //for (int i = 0; i < obj.inbox.Count; i++) {
            //    var mail = sib.CreateMail();
            //    mail.Id = obj.inbox[i].id;
            //    mail.DateTime = obj.inbox[i].datetime;
            //    mail.Title = obj.inbox[i].title;
            //    mail.Content = obj.inbox[i].content;
            //    sib.Add(mail);
            //}
        }
    }
}
