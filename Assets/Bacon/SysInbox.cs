using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bacon {
   public class SysInbox : IEnumerable<Sysmail> {
        private Dictionary<long, Sysmail> _dic = new Dictionary<long, Sysmail>();
        private List<Sysmail> _li = new List<Sysmail>();

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

        IEnumerator IEnumerable.GetEnumerator() {
            for (int i = 0; i < _li.Count; i++) {
                yield return _li[i];
            }
        }

        IEnumerator<Sysmail> IEnumerable<Sysmail>.GetEnumerator() {
            for (int i = 0; i < _li.Count; i++) {
                yield return _li[i];
            }
        }

        public int Count { get { return _li.Count; } }



    }
}
