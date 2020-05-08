using System;

namespace App
{
    public class AnnotationObjectEntity
    {
        public Guid Id { get; private set; }
        public int PageNumber { get; private set; }
        public bool AnnotationType { get; private set; }

        public AnnotationEntity Annotation { get; }

        protected AnnotationObjectEntity()
        {

        }

        public AnnotationObjectEntity(Guid id, int pageNumber, bool annotationType) : this()
        {
            Id = id;
            PageNumber = pageNumber;
            AnnotationType = annotationType;
        }

        public void Update(AnnotationObjectEntity annotationObject)
        {
            PageNumber = annotationObject.PageNumber;
            AnnotationType = annotationObject.AnnotationType;
        }
    }
}
