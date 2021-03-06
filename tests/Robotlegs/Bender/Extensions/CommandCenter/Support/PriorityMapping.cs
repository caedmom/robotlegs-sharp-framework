//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using Robotlegs.Bender.Extensions.CommandCenter.Impl;

namespace Robotlegs.Bender.Extensions.CommandCenter.Support
{
	public class PriorityMapping : CommandMapping
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public int priority;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public PriorityMapping (Type command, int priority) : base(command)
		{
			this.priority = priority;
		}
	}
}

