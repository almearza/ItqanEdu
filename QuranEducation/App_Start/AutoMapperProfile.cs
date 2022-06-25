using AutoMapper;
using QuranEducation.Models.VM;
using QuranEducation.Models;

namespace Amana.App_Start
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Tutorial, TutorialVM>();
            CreateMap<TutorialVM, Tutorial>();


            CreateMap<ApplicationUser, InstVM>();
            CreateMap<InstVM, ApplicationUser>();

            CreateMap<InstructorProfile, InstructorProfileVm>();
            CreateMap<InstructorProfileVm, InstructorProfile>();


            CreateMap<Inbox, InboxVM>();
            CreateMap<InboxVM, Inbox>();

            CreateMap<Lecture, LectureVM>();
            CreateMap<LectureVM, Lecture>();
            CreateMap<Assigment, AssigmentVM>();
            CreateMap<AssigmentVM, Assigment>();
            CreateMap<AssSolution, AssSolutionVM>();
            CreateMap<AssSolutionVM, AssSolution>();

        }
    }
}