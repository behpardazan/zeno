using Binbin.Linq;
using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_Post : BaseRepository<Post>
    {
        DatabaseEntities _context;
        public Entity_Post(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<Post> Search(int? websiteId = null, bool? active = null, int? notId = null, int? categoryId = null, string categoryLabel = null, int? typeId = null, int index = 1, int pageSize = 10, string keyword = null, string name = null, Enum_PostOrder order = Enum_PostOrder.NONE, Enum_Lang lang = Enum_Lang.NONE, string url = null, int? stateId = null, int? cityId = null)
        {
            var MyQuery = GetSearchQuery(websiteId: websiteId, active: active, notId: notId, categoryId: categoryId, categoryLabel: categoryLabel, typeId: typeId, keyword: keyword, name: name, lang: lang, url: url, stateId: stateId, cityId: cityId);
            int skipValue = pageSize * (index - 1);
            int pageValue = pageSize;

            if (order == Enum_PostOrder.NEW)
                return _context.Post.OrderByDescending(p => p.ID).Where(MyQuery).Skip(skipValue).Take(pageValue).ToList();
            else if (order == Enum_PostOrder.OLD)
                return _context.Post.OrderBy(p => p.ID).Where(MyQuery).Skip(skipValue).Take(pageValue).ToList();
            else if (order == Enum_PostOrder.MORE_VISIT)
                return _context.Post.OrderByDescending(p => p.VisitCount).Where(MyQuery).Skip(skipValue).Take(pageValue).ToList();
            else if (order == Enum_PostOrder.NAME)
                return _context.Post.OrderBy(p => p.Name).Where(MyQuery).Skip(skipValue).Take(pageValue).ToList();
            else
                return _context.Post.OrderByDescending(p => p.ID).Where(MyQuery).Skip(skipValue).Take(pageValue).ToList();
        }

        public int SearchCount(int? websiteId = null, int? notId = null, int? categoryId = null, string categoryLabel = null, string keyword = null, string name = null, Enum_Lang lang = Enum_Lang.NONE, int? stateId = null)
        {
            var MyQuery = GetSearchQuery(websiteId: websiteId, notId: notId, categoryId: categoryId, categoryLabel: categoryLabel, keyword: keyword, name: name, stateId: stateId);
            return _context.Post.Count(MyQuery);
        }

        private Expression<Func<Post, bool>> GetSearchQuery(int? websiteId = null, bool? active = null, int? notId = null, int? categoryId = null, string categoryLabel = null, int? typeId = null, string keyword = null, string name = null, Enum_Lang lang = Enum_Lang.NONE, string url = null, int? stateId = null, int? cityId = null)
        {
            var MyQuery = PredicateBuilder.True<Post>();
            if (notId != null && notId != 0)
                MyQuery = MyQuery.And(p => p.ID != notId);
            if (websiteId != null && websiteId != 0)
                MyQuery = MyQuery.And(p => p.WebsiteId == websiteId);
            if (categoryId != null && categoryId != 0)
                MyQuery = MyQuery.And(p => p.CategoryId == categoryId);
            if (categoryLabel != null)
                MyQuery = MyQuery.And(p => p.Category.Label == categoryLabel);
            if (typeId != null && typeId != 0)
                MyQuery = MyQuery.And(p => p.Category.TypeId == typeId);
            if (keyword != null)
                MyQuery = MyQuery.And(p => p.Keywords.Contains(keyword));
            if (url != null)
                MyQuery = MyQuery.And(p => p.Category.URL.Contains(url));
            if (active == true)
            {
                MyQuery = MyQuery.And(p => p.Active == true);

            }
            if (stateId != null && stateId != 0)
                MyQuery = MyQuery.And(p => p.StateId == stateId);
            if (cityId != null && cityId != 0)
                MyQuery = MyQuery.And(p => p.CityId == cityId);
            if (IsNullOrEmptyId(name) == false)
            {
                name = name.ToPersianChar();
                var nameQuery = PredicateBuilder.True<Post>();

                string[] nameSplit = name.Split(' ');
                foreach (var item in nameSplit)
                {
                    nameQuery = nameQuery.And(p =>
                    p.Name.Contains(item) ||
                    p.PostLanguage.Any(ss => ss.Name.Contains(item) || (ss.Keywords.Contains(item)) || (ss.Summary.Contains(item))) ||
                    (p.Keywords.Contains(item)) ||
                    (p.Summary.Contains(item)));
                }
                MyQuery = MyQuery.And(nameQuery);
            }
            return MyQuery;
        }
    }
}
