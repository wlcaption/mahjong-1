using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maria;
using Sproto;

namespace Bacon.Model {

    public class UComRecordMgr : Component  {

        private Dictionary<long, Record> _dic = new Dictionary<long, Record>();
        private List<Record> _li = new List<Record>();

        public UComRecordMgr(Entity entity) : base(entity) {
        }

        public IEnumerator<Record> GetEnumerator() {
            for (int i = 0; i < _li.Count; i++) {
                yield return _li[i];
            }
        }

        //IEnumerator IEnumerable.GetEnumerator() {
        //    for (int i = 0; i < _li.Count; i++) {
        //        yield return _li[i];
        //    }
        //}

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

        public void FetchRecords(SprotoTypeBase responseObj) {
            //C2sSprotoType.records.response obj = responseObj as C2sSprotoType.records.response;

            //AppContext ctx = _ctx as AppContext;
            //EntityMgr mgr = ctx.GetEntityMgr();
            //Entity entity = mgr.MyEntity;
            //Bacon.Model.RecordMgr recordmgr = entity.GetComponent<Bacon.Model.RecordMgr>();

            //for (int i = 0; i < obj.records.Count; i++) {
            //    Record record = new Bacon.Record();
            //    record.Id = obj.records[i].id;
            //    record.DateTime = obj.records[i].datetime;
            //    record.Player1 = obj.records[i].player1;
            //    record.Player2 = obj.records[i].player2;
            //    record.Player3 = obj.records[i].player3;
            //    record.Player4 = obj.records[i].player4;
            //    recordmgr.Add(record);
            //}
            //// 更新红点
            //_ctx.EnqueueRenderQueue(RenderFetchSysmail);
        }
    }
}
