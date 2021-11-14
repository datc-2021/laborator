using L05.Repository;

namespace L05
{
    class Program
    {
        private static IStudentsRepository _studentsRepository;
        private static IMetricRepository _metricRepository;

        static void Main()
        {
            _studentsRepository = new StudentsRepository();
            _metricRepository = new MetricRepository(_studentsRepository.GetAllStudents().Result);
            _metricRepository.GenerateMetric();
            
        }
    }
}
