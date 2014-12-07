﻿using System;
using NUnit.Framework;
using robotlegs.bender.framework.api;
using robotlegs.bender.framework.impl.guardSupport;

namespace robotlegs.bender.framework.impl
{
	public class GuardsApproveTest
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IInjector injector;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void before()
		{
			injector = new RobotlegsInjector();
		}

		[TearDown]
		public void after()
		{
			injector = null;
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void grumpy_Function_returns_false()
		{
			Assert.False (Guards.Approve (new object[]{ (Func<bool>)grumpyFunction }));
		}

		[Test]
		public void happy_Function_returns_true()
		{
			Assert.True (Guards.Approve ((Func<bool>)happyFunction));
		}

		[Test]
		public void grumpy_Class_returns_false()
		{
			Assert.False(Guards.Approve(typeof(GrumpyGuard)));
		}

		[Test]
		public void happy_Class_returns_true()
		{
			Assert.True(Guards.Approve(typeof(HappyGuard)));
		}

		[Test]
		public void grumpy_Instance_returns_false()
		{
			Assert.False(Guards.Approve(new GrumpyGuard()));
		}

		[Test]
		public void happy_Instance_returns_true()
		{
			Assert.True(Guards.Approve(new HappyGuard()));
		}

		[Test]
		public void guard_with_injections_returns_false_if_injected_guard_says_so()
		{
			injector.Map<BossGuard>().ToValue(new BossGuard(false));
			Assert.False(Guards.Approve(injector, typeof(JustTheMiddleManGuard)));
		}

		[Test]
		public void guard_with_injections_returns_true_if_injected_guard_says_so()
		{
			injector.Map<BossGuard>().ToValue(new BossGuard(true));
			Assert.True(Guards.Approve(injector, typeof(JustTheMiddleManGuard)));
		}

		[Test]
		public void guards_with_a_grumpy_Class_returns_false()
		{
			Assert.False(Guards.Approve (typeof(HappyGuard), typeof(GrumpyGuard)));
		}

		[Test]
		public void guards_with_a_grumpy_Instance_returns_false()
		{
			Assert.False(Guards.Approve (new HappyGuard(), new GrumpyGuard()));
		}

		[Test]
		public void guards_with_a_grumpy_Function_returns_false()
		{
			Assert.False(Guards.Approve (happyFunction, grumpyFunction));
		}

		[Test]
		public void falsey_Function_returns_false()
		{
			Func<bool> falseyGuard = delegate () {
				return false;
			};
			Assert.False (Guards.Approve(falseyGuard));
		}

		// Remove test due to no dynamic objects in language
		/*
		[Test]
		public void falsey_approve_returns_false()
		{
			var falseyGuard:Object = {
				approve: function():int {
					return 0;
				}
			};
			Assert.False (Guards.Approve (falseyGuard));
		}
		*/

		[Test, ExpectedException(typeof(NullReferenceException))]
		public void guard_instance_without_approve_throws_error()
		{
			object invalidGuard = new object ();
			Guards.Approve(invalidGuard);
			// note: no assertion. we just want to know if an error is thrown
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/
		
		private bool happyFunction()
		{
			return true;
		}

		private bool grumpyFunction()
		{
			return false;
		}
	}
}
