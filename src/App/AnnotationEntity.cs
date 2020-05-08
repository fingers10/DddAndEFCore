using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace App
{
    public class AnnotationEntity
    {
        public int Id { get; private set; }
        public string Value { get; private set; }

        private readonly List<AnnotationObjectEntity> _annotationObjects = new List<AnnotationObjectEntity>();
        public virtual IReadOnlyList<AnnotationObjectEntity> AnnotationObjects => _annotationObjects.ToList();

        protected AnnotationEntity()
        {

        }

        private AnnotationEntity(string value) : this()
        {
            Value = value;
        }

        private void UpdateContainer(string container)
        {
            Value = container;
        }

        public void AddAnnotation(AnnotationObjectEntity annotationObject)
        {
            if (!_annotationObjects.Any(x => x.Id == annotationObject.Id))
            {
                var annotationObjectEntity = new AnnotationObjectEntity
                (
                    annotationObject.Id,
                    annotationObject.PageNumber,
                    annotationObject.AnnotationType
                );
                _annotationObjects.Add(annotationObjectEntity);
                return;
            }

            var existingAnnotation = _annotationObjects.FirstOrDefault(x => x.Id.Equals(annotationObject.Id));

            if (existingAnnotation != null)
            {
                existingAnnotation.Update(annotationObject);
            }
        }

        public void RemoveAnnotation(Guid id)
        {
            XDocument annotation = XDocument.Parse(Value);

            var annotationObjectEntity = _annotationObjects.FirstOrDefault(x => x.Id == id);

            if (annotationObjectEntity != null)
            {
                _annotationObjects.Remove(annotationObjectEntity);
                annotation.Descendants("Object").Where(x => x.Element("Guid").Value == annotationObjectEntity.Id.ToString()).Remove();
            }

            UpdateContainer(annotation.ToString());
        }

        public static AnnotationEntity Create(string annotation)
        {
            XDocument doc = XDocument.Parse(annotation);
            var documentAnnotations = new List<AnnotationEntity>();

            var annotationEntity = new AnnotationEntity
                (
                    annotation
                );

            doc.Descendants("Container").ToList().ForEach(container =>
            {
                container.Descendants("Object").ToList().ForEach(annotationObject =>
                {
                    var annotationObjectEntity = new AnnotationObjectEntity
                    (
                        Guid.Parse(annotationObject.Element("Guid").Value.ToString()),
                        Convert.ToInt32(container.Element("PageNumber").Value.ToString()),
                        true
                    );

                    annotationEntity.AddAnnotation(annotationObjectEntity);
                });

            });

            return annotationEntity;
        }

        public void UpdateAnnotation(string modifiedAnnotationXml)
        {
            var modifiedAnnotationEntity = Create(modifiedAnnotationXml);

            //Removing deleted entities
            var deletedAnnotationsId = _annotationObjects.Where(x => !modifiedAnnotationEntity.AnnotationObjects.Any(y => y.Id.Equals(x.Id))).Select(x => x.Id).ToList();

            foreach (var annotationId in deletedAnnotationsId)
            {
                RemoveAnnotation(annotationId);
            }

            XDocument annotation = XDocument.Parse(Value);
            XDocument modifiedAnnotation = XDocument.Parse(modifiedAnnotationXml);

            //update or add
            foreach (var annotationObject in modifiedAnnotationEntity.AnnotationObjects)
            {
                AddAnnotation(annotationObject);
                annotation.Descendants("Object").Where(x => x.Element("Guid").Value == annotationObject.Id.ToString()).Remove();
                // extract annotation object xml from modified xml and add to existing xml

                var modifiedAnnotationObject = modifiedAnnotation.Descendants("Object").SingleOrDefault(x => x.Element("Guid").Value == annotationObject.Id.ToString());
                annotation
                    .Descendants("Container").SingleOrDefault(x => x.Element("PageNumber").Value == annotationObject.PageNumber.ToString())
                    .Descendants("Objects").SingleOrDefault()
                    .Add(modifiedAnnotationObject);

                UpdateContainer(annotation.ToString());
            }

            Value = $"<?xml version=\"1.0\" encoding=\"utf-8\"?>{Regex.Replace(Value, @"\t|\n|\r|\s", "")}";
        }
    }
}
