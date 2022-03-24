using System;
using System.Collections.Generic;
using InternalPortal.ViewModels;
using Ofgem.API.GGSS.Domain.Enums;

namespace InternalPortal.Helpers
{
    public static class ApplicationStatusTransitionsHelper
    {
        public static List<ApplicationStatus> GetTransitionableStatuses(this ApplicationStatus status)
        {
            return status switch
            {
                ApplicationStatus.WithApplicant =>
                    new List<ApplicationStatus>()
                    {
                        ApplicationStatus.StageOneWithApplicant
                    },
                
                ApplicationStatus.StageOneSubmitted => 
                    new List<ApplicationStatus>
                    {
                        ApplicationStatus.StageOneInReview
                    },
                
                ApplicationStatus.StageOneInReview => 
                    new List<ApplicationStatus> { 
                        ApplicationStatus.StageOneApproved,
                        ApplicationStatus.StageOneWithApplicant,
                        ApplicationStatus.StageOneRejected
                    },
                
                ApplicationStatus.StageTwoSubmitted => 
                    new List<ApplicationStatus>
                    {
                        ApplicationStatus.StageTwoInReview
                    },
                
                ApplicationStatus.StageTwoInReview => 
                    new List<ApplicationStatus>
                    {
                        ApplicationStatus.StageTwoApproved,
                        ApplicationStatus.StageTwoWithApplicant,
                        ApplicationStatus.StageTwoRejected
                    },
                
                ApplicationStatus.StageThreeSubmitted => 
                    new List<ApplicationStatus>
                    {
                        ApplicationStatus.StageThreeInReview
                    },
                
                ApplicationStatus.StageThreeInReview => 
                    new List<ApplicationStatus>
                    {
                        ApplicationStatus.StageThreeApproved,
                        ApplicationStatus.StageThreeWithApplicant,
                        ApplicationStatus.Rejected
                    },
                
                ApplicationStatus.StageOneApproved => 
                    new List<ApplicationStatus>
                    {
                        ApplicationStatus.StageOneRejected
                    },
                
                ApplicationStatus.StageTwoApproved => 
                    new List<ApplicationStatus>
                    {
                        ApplicationStatus.StageTwoRejected
                    },
                
                ApplicationStatus.StageThreeApproved => 
                    new List<ApplicationStatus>
                    {
                        ApplicationStatus.Rejected
                    },

                ApplicationStatus.StageOneWithApplicant  => 
                    new List<ApplicationStatus>
                    {
                        ApplicationStatus.StageOneInReview,
                    },
                
                ApplicationStatus.StageTwoWithApplicant  => 
                    new List<ApplicationStatus>
                    {
                        ApplicationStatus.StageTwoInReview,
                    },
                
                ApplicationStatus.StageThreeWithApplicant  => 
                    new List<ApplicationStatus>
                    {
                        ApplicationStatus.StageThreeInReview,
                    },
                
                ApplicationStatus.StageOneRejected => 
                    new List<ApplicationStatus>
                    {
                        ApplicationStatus.StageOneApproved,
                    },
                
                ApplicationStatus.StageTwoRejected => 
                    new List<ApplicationStatus>
                    {
                        ApplicationStatus.StageTwoApproved,
                    },
                
                ApplicationStatus.Rejected => 
                    new List<ApplicationStatus>
                    {
                        ApplicationStatus.StageThreeApproved,
                    },
                
                ApplicationStatus.Draft => 
                    new List<ApplicationStatus> { },
                
                _ => 
                    new List<ApplicationStatus>() { }
            };
        }
    }
}