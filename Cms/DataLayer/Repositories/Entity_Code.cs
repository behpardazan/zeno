using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_Code : BaseRepository<Code>
    {
        DatabaseEntities _context;
        public Entity_Code(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<Code> GetAllByCodeGroup(Enum_CodeGroup group, bool HasDefault = false,string value=null)
        {
            string groupString = group.ToString();
            List<Code> list = new List<Code>();
            if (HasDefault == true)
                list.Add(new Code() { ID = 0, Name = "انتخاب" });
            list.AddRange(_context.Code.Where(p => p.CodeGroup.Label == groupString 
            && (value==null || p.Value.Contains(value))
            ).OrderBy(p => p.Value).ToList());
            return list;
        }

        public Code GetByLabel(Enum_Code code)
        {
            string label = code.ToString();
            return _context.Code.FirstOrDefault(p => p.Label == label);
        }

        public int GetIdByLabel(Enum_Code code)
        {
            string label = code.ToString();
            return _context.Code.FirstOrDefault(p => p.Label == label).ID;
        }

        public int GetIdByLabel(string code)
        {
            return _context.Code.FirstOrDefault(p => p.Label == code).ID;
        }
    }
}
