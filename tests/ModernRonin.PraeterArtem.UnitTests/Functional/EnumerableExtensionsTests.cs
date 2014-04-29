﻿using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using ModernRonin.PraeterArtem.Functional;
using NUnit.Framework;

namespace ModernRonin.PraeterArtem.UnitTests.Functional
{
    [TestFixture]
    public class EnumerableExtensionsTests
    {
        [TestCase(new int[] {}, true)]
        [TestCase(new[] {1, 2, 3}, false)]
        [TestCase(new[] {1}, false)]
        public void IsEmpty(int[] enumerable, bool expectedResult)
        {
            enumerable.IsEmpty().Should().Be(expectedResult);
        }
        [Test]
        public void UseIn_Calls_Action_For_Each_Element()
        {
            var used = new List<int>();
            new[] {1, 2, 3, 4, 5}.UseIn(used.Add);
            used.ShouldAllBeEquivalentTo(new[] {1, 2, 3, 4, 5});
        }
        [Test]
        public void UseIn_Never_Calls_Action_If_Enumerable_Is_Empty()
        {
            new int[0].UseIn(x => Assert.Fail());
        }
        [Test]
        public void ExecuteOn_Calls_All_Actions_With()
        {
            var sum = 0;
            new Action<int>[]
            {i => sum += i, i => sum += 10*i, i => sum += 100*i}.ExecuteOn(7);
            Assert.AreEqual(777, sum);
        }
        [Test]
        public void ToEnumerable_Wraps_A_Single_Element_Into_An_IEnumerable()
        {
            3.ToEnumerable()
             .Should()
             .BeAssignableTo<IEnumerable<int>>()
             .And.HaveCount(1)
             .And.Contain(3);
        }
        [Test]
        public void ToEnumerable_On_An_IEnumerable_Returns_That_IEnumerable()
        {
            var son = new TypeImplementingIEnumerableOfItself(null);
            var daughter = new TypeImplementingIEnumerableOfItself(null);
            var parent =
                new TypeImplementingIEnumerableOfItself(new[] {son, daughter});
            parent.ToEnumerable()
                  .ShouldAllBeEquivalentTo(new[] {son, daughter});
        }
        [Test]
        public void AddTo_Adds_All_Elements_To_Collection()
        {
            var target = new List<int>();
            new[] {1, 2, 3}.AddTo(target);
            target.ShouldAllBeEquivalentTo(new[] {1, 2, 3});
        }
        [Test]
        public void ExceptNullValues_Returns_All_Elements_Which_Are_Not_Null()
        {
            new[] {"alpha", null, "bravo", "charlie", null}.ExceptNullValues()
                                                           .ShouldAllBeEquivalentTo
                (new[] {"alpha", "bravo", "charlie"});
        }
        [Test]
        public void ButFirst_On_Empty_Enumerable_Returns_Empty_Enumerable()
        {
            Null.Enumerable<int>().ButFirst().Should().BeEmpty();
        }
        [Test]
        public void
            ButFirst_On_Enumerable_With_Single_Element_Returns_Empty_Enumerable
            ()
        {
            "alpha".ToEnumerable().ButFirst().Should().BeEmpty();
        }
        [Test]
        public void ButFirst_Returns_Elements_Except_First()
        {
            new[] {1, 2, 3, 4, 5}.ButFirst()
                                 .Should()
                                 .Equal(new[] {2, 3, 4, 5});
        }
        [Test]
        public void ButLast_On_Empty_Enumerable_Returns_Empty_Enumerable()
        {
            Null.Enumerable<int>().ButLast().Should().BeEmpty();
        }
        [Test]
        public void
            ButLast_On_Enumerable_With_Single_Element_Returns_Empty_Enumerable
            ()
        {
            7.ToEnumerable().ButLast().Should().BeEmpty();
        }
        [Test]
        public void ButLast_Returns_All_Elements_Except_The_Last()
        {
            new[] {1, 2, 3, 4, 5}.ButLast()
                                 .Should()
                                 .Equal(new[] {1, 2, 3, 4});
        }
        [Test]
        public void Except_On_Empty_Enumerable_Returns_Empty_Enumerable()
        {
            Null.Enumerable<int>().Except(3).Should().BeEmpty();
        }
        [Test]
        public void
            Except_On_Enumerable_Without_Excepted_Value_Returns_Enumerable()
        {
            new[] {1, 2, 3}.Except(7)
                           .ShouldAllBeEquivalentTo(new[] {1, 2, 3});
        }
        [Test]
        public void Except_Returns_All_Elements_Except_Value()
        {
            new[] {1, 2, 3}.Except(2).ShouldAllBeEquivalentTo(new[] {1, 3});
        }
        [Test]
        public void EqualTo()
        {
            new[] {13, 2, 7, 25, 2}.EqualTo(2)
                                   .ShouldAllBeEquivalentTo(new[] {2, 2});
        }
        [Test]
        public void GreaterThan()
        {
            new[] {13, 2, 7, 25}.GreaterThan(7)
                                .ShouldAllBeEquivalentTo(new[] {13, 25});
        }
        [Test]
        public void GreaterOrEqual()
        {
            new[] {13, 2, 7, 25}.GreaterThanOrEqualTo(7)
                                .ShouldAllBeEquivalentTo(new[] {7, 13, 25});
        }
        [Test]
        public void Smaller()
        {
            new[] {13, 2, 7, 25}.SmallerThan(7)
                                .ShouldAllBeEquivalentTo(new[] {2});
        }
        [Test]
        public void SmallerOrEqual()
        {
            new[] {13, 2, 7, 25}.SmallerThanOrEqualTo(7)
                                .ShouldAllBeEquivalentTo(new[] {2, 7});
        }
        [Test]
        public void SameAs()
        {
            var a = new object();
            new[] {new object(), a, new object(), a}.SameAs(a)
                                                    .ShouldAllBeEquivalentTo(
                                                                             new[
                                                                                 ]
                                                                             {
                                                                                 a,
                                                                                 a
                                                                             });
        }
        [Test]
        public void MinElement()
        {
            new[] {"abc", "aa", "abcd"}.MinElement(s => s.Length)
                                       .Should()
                                       .Be("aa");
        }
        [Test]
        public void
            MinElement_On_Empty_ValueType_Enumerable_Returns_ValueTypeDefault
            ()
        {
            Null.Enumerable<int>()
                .MinElement(Functions.Identity<int>())
                .Should()
                .Be(0);
        }
        [Test]
        public void MinElement_On_Empty_ReferenceType_Enumerable_Returns_Null
            ()
        {
            Null.Enumerable<object>()
                .MinElement(e => e.GetHashCode())
                .Should()
                .BeNull();
        }
        [Test]
        public void MaxElement()
        {
            new[] {"abc", "aa", "abcd"}.MaxElement(s => s.Length)
                                       .Should()
                                       .Be("abcd");
        }
        [Test]
        public void
            MaxElement_On_Empty_ValueType_Enumerable_Returns_ValueTypeDefault
            ()
        {
            Null.Enumerable<int>()
                .MaxElement(Functions.Identity<int>())
                .Should()
                .Be(0);
        }
        [Test]
        public void MaxElement_On_Empty_ReferenceType_Enumerable_Returns_Null
            ()
        {
            Null.Enumerable<object>()
                .MaxElement(e => e.GetHashCode())
                .Should()
                .BeNull();
        }
        [Test]
        public void Max_Returns_Greatest_Element()
        {
            new[] {17, 18, 3, 5}.Max(999).Should().Be(18);
        }
        [Test]
        public void Max_Returns_Passed_Parameter_If_Enumerable_Is_Empty()
        {
            Null.Enumerable<int>().Max(999).Should().Be(999);
        }
        [Test]
        public void Min_Returns_Smallest_Element()
        {
            new[] {17, 18, 3, 5}.Min(1).Should().Be(3);
        }
        [Test]
        public void Min_Returns_Passed_Parameter_If_Enumerable_Is_Empty()
        {
            Null.Enumerable<int>().Min(1).Should().Be(1);
        }

        /// <summary>
        ///     An example for tree-like data structures.
        /// </summary>
        class TypeImplementingIEnumerableOfItself :
            IEnumerable<TypeImplementingIEnumerableOfItself>
        {
            readonly IEnumerable<TypeImplementingIEnumerableOfItself>
                mChildren;
            public TypeImplementingIEnumerableOfItself(
                IEnumerable<TypeImplementingIEnumerableOfItself> children)
            {
                mChildren = children;
            }
            public IEnumerator<TypeImplementingIEnumerableOfItself>
                GetEnumerator()
            {
                return mChildren.GetEnumerator();
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}