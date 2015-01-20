﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using robotlegs.bender.framework.impl;
using robotlegs.bender.framework.api;
using robotlegs.bender.framework.impl.contextSupport;

namespace robotlegs.bender.framework.impl
{
	[TestFixture]
	public class ContextTest
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IContext context;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void before()
		{
			context = new Context();
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void can_instantiate()
		{
			Assert.That(context, Is.Not.Null);
			Assert.That (context, Is.InstanceOf<IContext> ());
		}

		[Test]
		public void extensions_are_installed()
		{
			IContext actual = null;
			IExtension extension = new CallbackExtension(
				delegate(IContext c) {
					actual = c;
				});
			context.Install(extension);
			Assert.That(actual, Is.EqualTo(context));
		}

		[Test]
		public void configs_are_installed()
		{
			bool installed = false;
			IConfig config = new CallbackConfig(
				delegate() {
					installed = true;
				});
			context.Configure(config);
			context.Initialize();
			Assert.That(installed, Is.True);
		}

		[Test]
		public void injector_is_mapped_into_itself()
		{
			IInjector injector = context.injector.GetInstance<IInjector>();
			Assert.That(injector, Is.EqualTo(context.injector));
		}

		[Test]
		public void detain_stores_the_instance()
		{
			object expected = new object();
			object actual = null;
			context.Detained += delegate(object obj) {
				actual = obj;
			};
			context.Detain(expected);
			Assert.AreEqual (actual, expected);
		}

		[Test]
		public void release_frees_up_the_instance()
		{
			object expected = new object ();
			object actual = null;
			context.Released += delegate(object obj) {
				actual = obj;
			};
			context.Detain (expected);
			context.Release (expected);

			Assert.AreEqual (actual, expected);
		}

		/*
		[Test]
		public void addChild_sets_child_parentInjector()
		{
			Context child = new Context();
			context.AddChild(child);
			Assert.That(child.injector.parent, Is.EqualTo(context.injector));
		}

		[Test]
		public function addChild_logs_warning_unless_child_is_uninitialized():void
		{
			var warning:LogParams = null;
			context.addLogTarget(new CallbackLogTarget(
				function(log:LogParams):void {
					(log.level == LogLevel.WARN) && (warning = log);
				}));
			const child:Context = new Context();
			child.initialize();
			context.addChild(child);
			assertThat(warning.message, containsString("must be uninitialized"));
			assertThat(warning.params, array(child));
		}

		[Test]
		public function addChild_logs_warning_if_child_parentInjector_is_already_set():void
		{
			var warning:LogParams = null;
			context.addLogTarget(new CallbackLogTarget(
				function(log:LogParams):void {
					(log.level == LogLevel.WARN) && (warning = log);
				}));
			const child:Context = new Context();
			child.injector.parent = new RobotlegsInjector();
			context.addChild(child);
			assertThat(warning.message, containsString("must not have a parent Injector"));
			assertThat(warning.params, array(child));
		}

		[Test]
		public function removeChild_logs_warning_if_child_is_NOT_a_child():void
		{
			var warning:LogParams = null;
			context.addLogTarget(new CallbackLogTarget(
				function(log:LogParams):void {
					(log.level == LogLevel.WARN) && (warning = log);
				}));
			const child:Context = new Context();
			context.removeChild(child);
			assertThat(warning.message, containsString("must be a child"));
			assertThat(warning.params, array(child, context));
		}

		[Test]
		public function removesChild_clears_child_parentInjector():void
		{
			const child:Context = new Context();
			context.addChild(child);
			context.removeChild(child);
			assertThat(child.injector.parent, nullValue());
		}

		[Test]
		public function child_is_removed_when_child_is_destroyed():void
		{
			const child:Context = new Context();
			context.addChild(child);
			child.initialize();
			child.destroy();
			assertThat(child.injector.parent, nullValue());
		}

		[Test]
		public function children_are_removed_when_parent_is_destroyed():void
		{
			const child1:Context = new Context();
			const child2:Context = new Context();
			context.addChild(child1);
			context.addChild(child2);
			context.initialize();
			context.destroy();
			assertThat(child1.injector.parent, nullValue());
			assertThat(child2.injector.parent, nullValue());
		}

		[Test]
		public function removed_child_is_not_removed_again_when_destroyed():void
		{
			var warning:LogParams = null;
			context.addLogTarget(new CallbackLogTarget(
				function(log:LogParams):void {
					(log.level == LogLevel.WARN) && (warning = log);
				}));
			const child:Context = new Context();
			context.addChild(child);
			child.initialize();
			context.removeChild(child);
			child.destroy();
			assertThat(warning, nullValue());
		}
		*/

		[Test]
		public void lifecycleEvents_are_propagated()
		{
			List<object> actual = new List<object> ();
			List<object> expected = new List<object>{ 
				"PRE_INITIALIZE",
				"INITIALIZE",
				"POST_INITIALIZE",
				"PRE_SUSPEND",
				"SUSPEND",
				"POST_SUSPEND",
				"PRE_RESUME",
				"RESUME",
				"POST_RESUME",
				"PRE_DESTROY",
				"DESTROY",
				"POST_DESTROY"
			};
			context.PRE_INITIALIZE += CreateValuePusher (actual, "PRE_INITIALIZE");
			context.INITIALIZE += CreateValuePusher (actual, "INITIALIZE");
			context.POST_INITIALIZE += CreateValuePusher (actual, "POST_INITIALIZE");
			context.PRE_SUSPEND += CreateValuePusher (actual, "PRE_SUSPEND");
			context.SUSPEND += CreateValuePusher (actual, "SUSPEND");
			context.POST_SUSPEND += CreateValuePusher (actual, "POST_SUSPEND");
			context.PRE_RESUME += CreateValuePusher (actual, "PRE_RESUME");
			context.RESUME += CreateValuePusher (actual, "RESUME");
			context.POST_RESUME += CreateValuePusher (actual, "POST_RESUME");
			context.PRE_DESTROY += CreateValuePusher (actual, "PRE_DESTROY");
			context.DESTROY += CreateValuePusher (actual, "DESTROY");
			context.POST_DESTROY += CreateValuePusher (actual, "POST_DESTROY");

			context.Initialize();
			context.Suspend();
			context.Resume();
			context.Destroy();
			Assert.That(actual, Is.EqualTo(expected).AsCollection);
		}

		/*
		[Test]
		public function lifecycleStateChangeEvent_is_propagated():void
		{
			var called:Boolean = false;
			context.addEventListener(LifecycleEvent.STATE_CHANGE, function(event:LifecycleEvent):void {
				called = true;
			});
			context.initialize();
			assertThat(called, isTrue());
		}
		*/

		private Action CreateValuePusher(List<object> list, object value)
		{
			return delegate() {
				list.Add(value);
			};
		}
	}
}

