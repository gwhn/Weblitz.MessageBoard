using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using MvcContrib.TestHelper;
using NHibernate.Context;
using NUnit.Framework;
using StoryQ.Formatting.Parameters;
using Weblitz.MessageBoard.Core.Domain.Model;
using Weblitz.MessageBoard.Infrastructure.NHibernate;
using Weblitz.MessageBoard.Tests.Fixtures;

namespace Weblitz.MessageBoard.Tests.Controllers
{
    [TestFixture]
    public class ControllerTestBase : TestBase
    {
        protected ActionResult Result;

        protected Controller Controller;
        
        protected Guid ForumId;
        protected Forum Forum;

        protected Guid TopicId;
        protected Topic Topic;

        protected IList<Forum> Forums;

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            var session = SessionManager.Instance.OpenSession();

            CallSessionContext.Bind(session);
        }

        [TestFixtureTearDown]
        public void TestFixtureTeardown()
        {
            var session = CallSessionContext.Unbind(SessionManager.Instance.SessionFactory);

            if (session == null) return;
            
            if (session.Transaction != null && session.Transaction.IsActive)
            {
                session.Transaction.Rollback();
            }
            else
            {
                session.Flush();                    
            }
            
            session.Close();
            session.Dispose();
        }

        protected void ListWith_Forums(int count)
        {
            Forums = new List<Forum>();

            for (var i = 0; i < count; i++)
            {
                Forums.Add(ForumFixtures.ForumWithNoTopics(i));
            }
        }

        protected void EachForumContains_Topics(int count)
        {
            foreach (var forum in Forums)
            {
                for (var i = 0; i < count; i++)
                {
                    forum.Add(TopicFixtures.TopicWithNoPostsAndNoAttachments(i));
                }
            }
        }

        protected void EachTopicContains_Posts(int count)
        {
            foreach (var topic in Forums.SelectMany(forum => forum.Topics))
            {
                for (var i = 0; i < count; i++)
                {
                    topic.Add(PostFixtures.RootPostWithNoChildren(i));
                }
            }
        }

        protected void IdOfForumThat_Exist([BooleanParameterFormat("does", "does not")] bool exists)
        {
            if (exists)
            {
                ForumId = Guid.NewGuid();
                Forum = ForumFixtures.ForumWithNoTopics(ForumId);
            }
            else
            {
                ForumId = Guid.Empty;
                Forum = default(Forum);
            }
        }

        protected void IdOfTopicThat_Exist([BooleanParameterFormat("does", "does not")] bool exists)
        {
            if (exists)
            {
                TopicId = Guid.NewGuid();
                Topic = TopicFixtures.TopicWithNoPostsAndNoAttachments(TopicId);

                ForumId = Guid.NewGuid();
                Topic.Forum = ForumFixtures.ForumWithNoTopics(ForumId);
            }
            else
            {
                TopicId = Guid.Empty;
                Topic = default(Topic);
            }
        }

        protected void ShouldRenderDefaultView()
        {
            var result = Result as ViewResult;

            Assert.IsNotNull(result);

            Assert.That(result.AssertViewRendered().ViewName == string.Empty);
        }

        protected void ShouldReturnViewResult()
        {
            Assert.IsNotNull(Result);
            Assert.IsInstanceOf<ViewResult>(Result);
        }

        protected void ShouldReturnRedirectToRouteResult()
        {
            Assert.IsNotNull(Result);
            Assert.IsInstanceOf<RedirectToRouteResult>(Result);
        }

        protected void SetEntityId(Entity entity, Guid id)
        {
            if (entity == null) return;
            
            entity.GetType()
                .GetProperty("Id", BindingFlags.Instance | BindingFlags.Public)
                .SetValue(entity, id, null);
        }

        protected void ViewModel_Contain<T>([BooleanParameterFormat("should", "should not")] bool contains)
        {
            var result = Result as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ViewData);
            Assert.IsNotNull(result.ViewData.Model);

            if (contains)
            {
                Assert.IsInstanceOf<T>(result.ViewData.Model);
            }
            else
            {
                Assert.IsNotInstanceOf<T>(result.ViewData.Model);
            }
        }

        protected void Message_Contain_([BooleanParameterFormat("should", "should not")] bool contains, string text)
        {
            if (contains)
            {
                Assert.That(Controller.TempData.ContainsKey("Message"));

                var message = Controller.TempData["Message"] as string;
                Assert.IsNotNull(message);

                Assert.That(message.ToLowerInvariant().Contains(text.ToLowerInvariant()));
            }
            else
            {
                Assert.IsFalse(Controller.TempData.ContainsKey("Message"));
            }
        }

        protected void ShouldRedirectTo__(string controller, string action)
        {
            var result = Result as RedirectToRouteResult;
            Assert.IsNotNull(result);

            Assert.That(result.RouteValues["Controller"].ToString() == controller);
            Assert.That(result.RouteValues["Action"].ToString() == action);
        }
    }
}