using Assert.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.DTOs.Responses
{
    public class ComplaintReasonTreeDTO
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool RequiresFreeText { get; set; }
        public int SeverityLevel { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? ParentId { get; set; }
        public int Level { get; set; }
        public string HierarchyPath { get; set; } = string.Empty;
        public string FullDescriptionPath { get; set; } = string.Empty;
        public bool HasChildren { get; set; }
        public List<ComplaintReasonTreeDTO> Children { get; set; } = new List<ComplaintReasonTreeDTO>();

    }

    public class ComplaintReasonUtil
    {
        public List<ComplaintReasonTreeDTO> BuildTree(IEnumerable<AppComplaintReasonHierarchyDto> flatList)
        {
            //var lookup = flatList.ToLookup(x => x.ParentId);

            //List<ComplaintReasonTreeDTO> BuildTreeNodes(int? parentId)
            //{
            //    return lookup[parentId]
            //        .Select(item => new ComplaintReasonTreeDTO
            //        {
            //            Id = item.ComplaintReasonId,
            //            Code = item.ComplaintReasonCode,
            //            Description = item.ReasonDescription,
            //            IsActive = item.IsActive ?? true,
            //            RequiresFreeText = item.RequiresFreeText ?? false,
            //            SeverityLevel = item.SeverityLevel ?? 1,
            //            CreatedAt = item.CreatedAt ?? DateTime.UtcNow,
            //            ParentId = item.ParentId,
            //            Level = item.Level,
            //            HierarchyPath = item.HierarchyPath,
            //            FullDescriptionPath = item.FullDescriptionPath,
            //            HasChildren = lookup[item.ComplaintReasonId].Any(),
            //            Children = BuildTreeNodes(item.ComplaintReasonId)
            //        })
            //        .OrderBy(x => x.Code)
            //        .ToList();
            //}

            //return BuildTreeNodes(null); // Raíces (parentId = null)
            var lookup = flatList.ToLookup(x => x.ParentId);

            List<ComplaintReasonTreeDTO> BuildTreeNodes(int? parentId)
            {
                return lookup[parentId]
                    .OrderBy(x => x.ComplaintReasonId) // Ordenar por ID principal
                    .Select(item => new ComplaintReasonTreeDTO
                    {
                        Id = item.ComplaintReasonId,
                        Code = item.ComplaintReasonCode,
                        Description = item.ReasonDescription,
                        IsActive = item.IsActive ?? true,
                        RequiresFreeText = item.RequiresFreeText ?? false,
                        SeverityLevel = item.SeverityLevel ?? 1,
                        CreatedAt = item.CreatedAt ?? DateTime.UtcNow,
                        ParentId = item.ParentId,
                        Level = item.Level,
                        HierarchyPath = item.HierarchyPath,
                        FullDescriptionPath = item.FullDescriptionPath,
                        HasChildren = lookup[item.ComplaintReasonId].Any(),
                        Children = BuildTreeNodes(item.ComplaintReasonId) // Esto ordenará recursivamente los hijos
                    })
                    .ToList();
            }

            return BuildTreeNodes(null);
        }
    }
}
