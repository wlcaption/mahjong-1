using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bacon {
    public class RecordMgr : IEnumerable<Record> {

        private Dictionary<long, Record> _dic = new Dictionary<long, Record>();
        private List<Record> _li = new List<Record>();

        public IEnumerator<Record> GetEnumerator() {
            for (int i = 0; i < _li.Count; i++) {
                yield return _li[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            for (int i = 0; i < _li.Count; i++) {
                yield return _li[i];
            }
        }

        public void Add(Record record) {
            _dic.Add(record.Id, record);
            _li.Add(record);
        }

        public void Remove(Record record) {
            _dic.Remove(record.Id);
            _li.Remove(record);
        }

        public Record GetRecord(long id) {
            return _dic[id];
        }
    }
}
