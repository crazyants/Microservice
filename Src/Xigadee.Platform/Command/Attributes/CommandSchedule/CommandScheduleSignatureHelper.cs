﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Xigadee
{
    /// <summary>
    /// This helper returns the appropriate helpers for the commands that use attributes to map a command method.
    /// </summary>
    public static class CommandScheduleSignatureHelper
    {

        /// <summary>
        /// This static helper returns the list of attributes, methods and references. Not one method may have multiple attributes assigned to them.
        /// </summary>
        public static List<Tuple<CommandScheduleAttribute, ScheduleMethodSignature, string>> ScheduleMethodAttributeSignatures(
            this ICommand command, bool throwExceptions = false)
        {
            return command
                .ScheduleMethodSignatures(throwExceptions)
                .SelectMany((s) => s.CommandAttributes.Select((a) => new Tuple<CommandScheduleAttribute, ScheduleMethodSignature, string>(a, s, s.Reference(a))))
                .ToList();
        }

        /// <summary>
        /// This static helper returns the 
        /// </summary>
        public static List<ScheduleMethodSignature> ScheduleMethodSignatures(this ICommand command, bool throwExceptions)
        {
            var results = command.CommandMethods()
                .Select((m) => new ScheduleMethodSignature(command, m, throwExceptions))
                .Where((t) => t.IsValid)
                .ToList();

            return results;
        }

        /// <summary>
        /// This static helper returns the list of methods that are decorated with a CommandContractAttribute attribute.
        /// </summary>
        public static List<MethodInfo> ScheduleMethods(this ICommand command)
        {
            return command.GetType().ScheduleMethods();
        }

        /// <summary>
        /// This static helper returns the list of methods that are decorated with CommandContractAttributes
        /// </summary>
        public static List<MethodInfo> ScheduleMethods(this Type objectType)
        {
            var results = objectType
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where((m) => m.CustomAttributes.Count((a) => a.AttributeType == typeof(CommandScheduleAttribute)) > 0)
                .ToList();

            return results;
        }
    }
}
