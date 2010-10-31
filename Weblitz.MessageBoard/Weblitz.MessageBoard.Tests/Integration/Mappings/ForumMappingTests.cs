using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using StoryQ;
using StoryQ.Formatting.Parameters;
using Weblitz.MessageBoard.Core.Domain.Model;
using Weblitz.MessageBoard.Tests.Fixtures;

namespace Weblitz.MessageBoard.Tests.Integration.Mappings
{
    [TestFixture]
    public class ForumMappingTests : DataTestBase
    {
        private Forum _forum;
        private Guid _id;
        private readonly IList<Topic> _added = new List<Topic>();
        private readonly IList<Topic> _removed = new List<Topic>();

        private const string Opened = "Opened";
        private const string Closed = "Closed";
        private const string Loaded = "Loaded";
        private const string Saved = "Saved";
        private const string Modified = "Modified";
        private const string Deleted = "Deleted";

        [Test]
        public void ForumMapping()
        {
            new Story("forum mapping")
                .InOrderTo("check forum persistence")
                .AsA("developer")
                .IWant("to create, read, update and delete forums")

                        .WithScenario("create forum with no associated topics")
                            .Given(ForumWith_Topics, 0)
                                .And(SessionIs_, Opened)
                            .When(ForumIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(LoadedForum_MatchSavedForum, true)
                                .And(SessionIs_, Closed)

                        .WithScenario("create forum with associated topics")
                            .Given(ForumWith_Topics, 1)
                                .And(SessionIs_, Opened)
                            .When(ForumIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(LoadedForum_MatchSavedForum, true)
                                .And(SessionIs_, Closed)

                        .WithScenario("update forum with no associated topics")
                            .Given(ForumWith_Topics, 0)
                                .And(SessionIs_, Opened)
                                .And(ForumIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .When(ForumIs_, Loaded)
                                .And(ForumIs_, Modified)
                                .And(ForumIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(LoadedForum_MatchSavedForum, true)
                                .And(SessionIs_, Closed)

                        .WithScenario("update forum with topics added")
                            .Given(ForumWith_Topics, 0)
                                .And(SessionIs_, Opened)
                                .And(ForumIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .When(ForumIs_, Loaded)
                                .And(ForumIs_, Modified)
                                .And(_TopicsAddedToForum, 2)
                                .And(ForumIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(LoadedForum_MatchSavedForum, true)
                                .And(Forum_ContainAddedTopics, true)
                                .And(SessionIs_, Closed)

                        .WithScenario("update forum with topics removed")
                            .Given(ForumWith_Topics, 2)
                                .And(SessionIs_, Opened)
                                .And(ForumIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .When(ForumIs_, Loaded)
                                .And(ForumIs_, Modified)
                                .And(_TopicsRemovedFromForum, 1)
                                .And(ForumIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(LoadedForum_MatchSavedForum, true)
                                .And(Forum_ContainRemovedTopics, false)
                                .And(SessionIs_, Closed)

                        .WithScenario("delete forum with no associated topics")
                            .Given(ForumWith_Topics, 0)
                                .And(SessionIs_, Opened)
                                .And(ForumIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .When(ForumIs_, Deleted)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(Forum_Exist, false)
                                .And(SessionIs_, Closed)

                        .WithScenario("delete forum with associated topics")
                            .Given(ForumWith_Topics, 2)
                                .And(SessionIs_, Opened)
                                .And(ForumIs_, Saved)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .When(ForumIs_, Deleted)
                                .And(SessionIs_, Closed)
                                .And(SessionIs_, Opened)
                            .Then(Forum_Exist, false)
                                .And(AssociatedTopics_Exist, false)
                                .And(SessionIs_, Closed)
                .Execute();
        }

        private void ForumWith_Topics(int count)
        {
            _forum = ForumFixtures.ForumWithNoTopics;
            if (count < 1) return;
            for (var i = 0; i < count; i++)
            {
                _forum.Add(TopicFixtures.TopicWithNoPostsAndNoAttachments(i));
            }
        }

        private void SessionIs_(string action)
        {
            switch (action)
            {
                case Opened:
                    Session = BuildSession();
                    Assert.That(Session.IsOpen);
                    break;

                case Closed:
                    Session.Close();
                    Assert.That(!Session.IsOpen);
                    Session.Dispose();
                    break;
            }
        }

        private void ForumIs_(string action)
        {
            switch (action)
            {
                case Loaded:
                    _forum = Session.Load<Forum>(_id);
                    break;

                case Saved:
                    Session.SaveOrUpdate(_forum);
                    Assert.That(Session.IsDirty());
                    Session.Flush();
                    _id = _forum.Id;
                    break;

                case Modified:
                    _forum.Name =string.Format("{0} {1}", _forum.Name, Modified);
                    break;

                case Deleted:
                    Session.Delete(_forum);
                    Assert.That(Session.IsDirty());
                    Session.Flush();
                    break;
            }
        }

        private void LoadedForum_MatchSavedForum([BooleanParameterFormat("should", "should not")] bool matches)
        {
            var actual = Session.Load<Forum>(_id);

            Assert.That(actual, Is.EqualTo(_forum));
            Assert.That(actual, Is.Not.SameAs(_forum));
        }

        private void _TopicsAddedToForum(int count)
        {
            Assert.That(count > 0);
            Assert.IsNotNull(_forum);

            for (var i = 0; i < count; i++)
            {
                var topic = TopicFixtures.TopicWithNoPostsAndNoAttachments(i);
                _added.Add(topic);
                _forum.Add(topic);
            }
        }

        private void _TopicsRemovedFromForum(int count)
        {
            Assert.That(count <= _forum.Topics.Count());

            for (var i = 0; i < count; i++)
            {
                var topic = _forum.Topics[0];
                _removed.Add(topic);
                _forum.Remove(topic);
            }
        }

        private void Forum_ContainAddedTopics([BooleanParameterFormat("should", "should not")] bool contains)
        {
            var actual = Session.Get<Forum>(_id);

            foreach (var topic in actual.Topics)
            {
                if (contains)
                {
                    Assert.IsTrue(_added.Contains(topic));
                }
                else
                {
                    Assert.IsFalse(_added.Contains(topic));
                }
            }
        }

        private void Forum_ContainRemovedTopics([BooleanParameterFormat("should", "should not")] bool contains)
        {
            var actual = Session.Get<Forum>(_id);

            foreach (var topic in actual.Topics)
            {
                if (contains)
                {
                    Assert.IsTrue(_removed.Contains(topic));
                }
                else
                {
                    Assert.IsFalse(_removed.Contains(topic));
                }
            }
        }

        private void Forum_Exist([BooleanParameterFormat("should", "should not")] bool exists)
        {
            var actual = Session.Get<Forum>(_id);

            if (exists)
            {
                Assert.IsNotNull(actual);
            }
            else
            {
                Assert.IsNull(actual);
            }
        }

        private void AssociatedTopics_Exist([BooleanParameterFormat("should", "should not")] bool exists)
        {
            foreach (var topic in _forum.Topics)
            {
                var actual = Session.Get<Topic>(topic.Id);

                if (exists)
                {
                    Assert.IsNotNull(actual);
                }
                else
                {
                    Assert.IsNull(actual);
                }
            }
        }
    }
}