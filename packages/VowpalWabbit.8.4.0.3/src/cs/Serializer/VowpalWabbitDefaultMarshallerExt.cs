﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VowpalWabbitInterfaceVisitorExt.cs">
//   Copyright (c) by respective owners including Yahoo!, Microsoft, and
//   individual contributors. All rights reserved.  Released under a BSD
//   license as described in the file LICENSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Globalization;
using System.Text;
using VW.Serializer.Intermediate;


namespace VW.Serializer
{
    public partial class VowpalWabbitDefaultMarshaller
    {
                /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, PreHashedFeature feature, System.Byte value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            
            context.NamespaceBuilder.AddFeature(feature.FeatureHash, (float)value);

            context.AppendStringExample(
                feature.Dictify,
                " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}"),
                feature.Name,
                value);
        }

        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        public unsafe void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, System.Byte[] value)
        {
            if (value == null)
                return;

            this.MarshalFeature(context, ns, feature, value, 0, value.Length);
        }

        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature values.</param>
        /// <param name="offset">Start offset for feature values.</param>
        /// <param name="length">Length of feature values.</param>
        public unsafe void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, System.Byte[] value, int offset, int length)
        {
            if (value == null)
                return;

            ulong i = 0;

            // support anchor feature
            if (feature.AddAnchor)
            {
                context.NamespaceBuilder.PreAllocate(value.Length + 1);

                context.NamespaceBuilder.AddFeature(ns.NamespaceHash, 1);
                i++;
            }
            else
                context.NamespaceBuilder.PreAllocate(value.Length);

            
            for (var j = offset;j<length;j++)
            {
                var v = value[j];

                
                context.NamespaceBuilder.AddFeature((ulong)(ns.NamespaceHash + i), (float)v);
                i++;
            }
            
            if (context.StringExample == null)
                return;

            string featureString;
            if (feature.Dictify && context.FastDictionary != null)
            {
                if (context.FastDictionary.TryGetValue(value, out featureString))
                {
                    context.AppendStringExample(feature.Dictify, featureString);
                    return;
                }
            }

            var featureBuilder = new StringBuilder();

            // support anchor feature
            i = 0;
            if (feature.AddAnchor)
            {
                featureBuilder.Append(" 0:1");
                i++;
            }

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            for (var j = offset;j<length;j++)
            {
                featureBuilder.AppendFormat(
                    CultureInfo.InvariantCulture,
                    format,
                    i,
                    value[j]);
                i++;
            }

            featureString = featureBuilder.ToString();

            if (feature.Dictify && context.FastDictionary != null)
                context.FastDictionary.Add(value, featureString);

            context.AppendStringExample(feature.Dictify, featureString);
        }

        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IEnumerable<System.Byte> value)
        {
            if (value == null)
            {
                return;
            }

            ulong i = 0;

            // support anchor feature
            if (feature.AddAnchor)
            {
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash, 1);
                i++;
            }

            foreach (var v in value)
            {
                
                context.NamespaceBuilder.AddFeature((ulong)(ns.NamespaceHash + i), (float)v);
                i++;
            }

            if (context.StringExample == null)
                return;

            string featureString;
            if (feature.Dictify && context.FastDictionary != null)
            {
                if (context.FastDictionary.TryGetValue(value, out featureString))
                {
                    context.AppendStringExample(feature.Dictify, featureString);
                    return;
                }
            }

            var featureBuilder = new StringBuilder();

            // support anchor feature
            i = 0;
            if (feature.AddAnchor)
            {
                featureBuilder.Append(" 0:1");
                i++;
            }

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var v in value)
            {
                featureBuilder.AppendFormat(
                    CultureInfo.InvariantCulture,
                    format,
                    i,
                    v);
                i++;
            }

            featureString = featureBuilder.ToString();

            if (feature.Dictify && context.FastDictionary != null)
                context.FastDictionary.Add(value, featureString);

            context.AppendStringExample(feature.Dictify, featureString);
        }

                /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, PreHashedFeature feature, System.SByte value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            
            context.NamespaceBuilder.AddFeature(feature.FeatureHash, (float)value);

            context.AppendStringExample(
                feature.Dictify,
                " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}"),
                feature.Name,
                value);
        }

        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        public unsafe void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, System.SByte[] value)
        {
            if (value == null)
                return;

            this.MarshalFeature(context, ns, feature, value, 0, value.Length);
        }

        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature values.</param>
        /// <param name="offset">Start offset for feature values.</param>
        /// <param name="length">Length of feature values.</param>
        public unsafe void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, System.SByte[] value, int offset, int length)
        {
            if (value == null)
                return;

            ulong i = 0;

            // support anchor feature
            if (feature.AddAnchor)
            {
                context.NamespaceBuilder.PreAllocate(value.Length + 1);

                context.NamespaceBuilder.AddFeature(ns.NamespaceHash, 1);
                i++;
            }
            else
                context.NamespaceBuilder.PreAllocate(value.Length);

            
            for (var j = offset;j<length;j++)
            {
                var v = value[j];

                
                context.NamespaceBuilder.AddFeature((ulong)(ns.NamespaceHash + i), (float)v);
                i++;
            }
            
            if (context.StringExample == null)
                return;

            string featureString;
            if (feature.Dictify && context.FastDictionary != null)
            {
                if (context.FastDictionary.TryGetValue(value, out featureString))
                {
                    context.AppendStringExample(feature.Dictify, featureString);
                    return;
                }
            }

            var featureBuilder = new StringBuilder();

            // support anchor feature
            i = 0;
            if (feature.AddAnchor)
            {
                featureBuilder.Append(" 0:1");
                i++;
            }

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            for (var j = offset;j<length;j++)
            {
                featureBuilder.AppendFormat(
                    CultureInfo.InvariantCulture,
                    format,
                    i,
                    value[j]);
                i++;
            }

            featureString = featureBuilder.ToString();

            if (feature.Dictify && context.FastDictionary != null)
                context.FastDictionary.Add(value, featureString);

            context.AppendStringExample(feature.Dictify, featureString);
        }

        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IEnumerable<System.SByte> value)
        {
            if (value == null)
            {
                return;
            }

            ulong i = 0;

            // support anchor feature
            if (feature.AddAnchor)
            {
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash, 1);
                i++;
            }

            foreach (var v in value)
            {
                
                context.NamespaceBuilder.AddFeature((ulong)(ns.NamespaceHash + i), (float)v);
                i++;
            }

            if (context.StringExample == null)
                return;

            string featureString;
            if (feature.Dictify && context.FastDictionary != null)
            {
                if (context.FastDictionary.TryGetValue(value, out featureString))
                {
                    context.AppendStringExample(feature.Dictify, featureString);
                    return;
                }
            }

            var featureBuilder = new StringBuilder();

            // support anchor feature
            i = 0;
            if (feature.AddAnchor)
            {
                featureBuilder.Append(" 0:1");
                i++;
            }

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var v in value)
            {
                featureBuilder.AppendFormat(
                    CultureInfo.InvariantCulture,
                    format,
                    i,
                    v);
                i++;
            }

            featureString = featureBuilder.ToString();

            if (feature.Dictify && context.FastDictionary != null)
                context.FastDictionary.Add(value, featureString);

            context.AppendStringExample(feature.Dictify, featureString);
        }

                /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, PreHashedFeature feature, System.Int16 value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            
            context.NamespaceBuilder.AddFeature(feature.FeatureHash, (float)value);

            context.AppendStringExample(
                feature.Dictify,
                " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}"),
                feature.Name,
                value);
        }

        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        public unsafe void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, System.Int16[] value)
        {
            if (value == null)
                return;

            this.MarshalFeature(context, ns, feature, value, 0, value.Length);
        }

        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature values.</param>
        /// <param name="offset">Start offset for feature values.</param>
        /// <param name="length">Length of feature values.</param>
        public unsafe void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, System.Int16[] value, int offset, int length)
        {
            if (value == null)
                return;

            ulong i = 0;

            // support anchor feature
            if (feature.AddAnchor)
            {
                context.NamespaceBuilder.PreAllocate(value.Length + 1);

                context.NamespaceBuilder.AddFeature(ns.NamespaceHash, 1);
                i++;
            }
            else
                context.NamespaceBuilder.PreAllocate(value.Length);

            
            for (var j = offset;j<length;j++)
            {
                var v = value[j];

                
                context.NamespaceBuilder.AddFeature((ulong)(ns.NamespaceHash + i), (float)v);
                i++;
            }
            
            if (context.StringExample == null)
                return;

            string featureString;
            if (feature.Dictify && context.FastDictionary != null)
            {
                if (context.FastDictionary.TryGetValue(value, out featureString))
                {
                    context.AppendStringExample(feature.Dictify, featureString);
                    return;
                }
            }

            var featureBuilder = new StringBuilder();

            // support anchor feature
            i = 0;
            if (feature.AddAnchor)
            {
                featureBuilder.Append(" 0:1");
                i++;
            }

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            for (var j = offset;j<length;j++)
            {
                featureBuilder.AppendFormat(
                    CultureInfo.InvariantCulture,
                    format,
                    i,
                    value[j]);
                i++;
            }

            featureString = featureBuilder.ToString();

            if (feature.Dictify && context.FastDictionary != null)
                context.FastDictionary.Add(value, featureString);

            context.AppendStringExample(feature.Dictify, featureString);
        }

        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IEnumerable<System.Int16> value)
        {
            if (value == null)
            {
                return;
            }

            ulong i = 0;

            // support anchor feature
            if (feature.AddAnchor)
            {
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash, 1);
                i++;
            }

            foreach (var v in value)
            {
                
                context.NamespaceBuilder.AddFeature((ulong)(ns.NamespaceHash + i), (float)v);
                i++;
            }

            if (context.StringExample == null)
                return;

            string featureString;
            if (feature.Dictify && context.FastDictionary != null)
            {
                if (context.FastDictionary.TryGetValue(value, out featureString))
                {
                    context.AppendStringExample(feature.Dictify, featureString);
                    return;
                }
            }

            var featureBuilder = new StringBuilder();

            // support anchor feature
            i = 0;
            if (feature.AddAnchor)
            {
                featureBuilder.Append(" 0:1");
                i++;
            }

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var v in value)
            {
                featureBuilder.AppendFormat(
                    CultureInfo.InvariantCulture,
                    format,
                    i,
                    v);
                i++;
            }

            featureString = featureBuilder.ToString();

            if (feature.Dictify && context.FastDictionary != null)
                context.FastDictionary.Add(value, featureString);

            context.AppendStringExample(feature.Dictify, featureString);
        }

                /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, PreHashedFeature feature, System.Int32 value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            
            context.NamespaceBuilder.AddFeature(feature.FeatureHash, (float)value);

            context.AppendStringExample(
                feature.Dictify,
                " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}"),
                feature.Name,
                value);
        }

        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        public unsafe void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, System.Int32[] value)
        {
            if (value == null)
                return;

            this.MarshalFeature(context, ns, feature, value, 0, value.Length);
        }

        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature values.</param>
        /// <param name="offset">Start offset for feature values.</param>
        /// <param name="length">Length of feature values.</param>
        public unsafe void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, System.Int32[] value, int offset, int length)
        {
            if (value == null)
                return;

            ulong i = 0;

            // support anchor feature
            if (feature.AddAnchor)
            {
                context.NamespaceBuilder.PreAllocate(value.Length + 1);

                context.NamespaceBuilder.AddFeature(ns.NamespaceHash, 1);
                i++;
            }
            else
                context.NamespaceBuilder.PreAllocate(value.Length);

            
            for (var j = offset;j<length;j++)
            {
                var v = value[j];

                
                context.NamespaceBuilder.AddFeature((ulong)(ns.NamespaceHash + i), (float)v);
                i++;
            }
            
            if (context.StringExample == null)
                return;

            string featureString;
            if (feature.Dictify && context.FastDictionary != null)
            {
                if (context.FastDictionary.TryGetValue(value, out featureString))
                {
                    context.AppendStringExample(feature.Dictify, featureString);
                    return;
                }
            }

            var featureBuilder = new StringBuilder();

            // support anchor feature
            i = 0;
            if (feature.AddAnchor)
            {
                featureBuilder.Append(" 0:1");
                i++;
            }

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            for (var j = offset;j<length;j++)
            {
                featureBuilder.AppendFormat(
                    CultureInfo.InvariantCulture,
                    format,
                    i,
                    value[j]);
                i++;
            }

            featureString = featureBuilder.ToString();

            if (feature.Dictify && context.FastDictionary != null)
                context.FastDictionary.Add(value, featureString);

            context.AppendStringExample(feature.Dictify, featureString);
        }

        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IEnumerable<System.Int32> value)
        {
            if (value == null)
            {
                return;
            }

            ulong i = 0;

            // support anchor feature
            if (feature.AddAnchor)
            {
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash, 1);
                i++;
            }

            foreach (var v in value)
            {
                
                context.NamespaceBuilder.AddFeature((ulong)(ns.NamespaceHash + i), (float)v);
                i++;
            }

            if (context.StringExample == null)
                return;

            string featureString;
            if (feature.Dictify && context.FastDictionary != null)
            {
                if (context.FastDictionary.TryGetValue(value, out featureString))
                {
                    context.AppendStringExample(feature.Dictify, featureString);
                    return;
                }
            }

            var featureBuilder = new StringBuilder();

            // support anchor feature
            i = 0;
            if (feature.AddAnchor)
            {
                featureBuilder.Append(" 0:1");
                i++;
            }

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var v in value)
            {
                featureBuilder.AppendFormat(
                    CultureInfo.InvariantCulture,
                    format,
                    i,
                    v);
                i++;
            }

            featureString = featureBuilder.ToString();

            if (feature.Dictify && context.FastDictionary != null)
                context.FastDictionary.Add(value, featureString);

            context.AppendStringExample(feature.Dictify, featureString);
        }

                /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, PreHashedFeature feature, System.UInt16 value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            
            context.NamespaceBuilder.AddFeature(feature.FeatureHash, (float)value);

            context.AppendStringExample(
                feature.Dictify,
                " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}"),
                feature.Name,
                value);
        }

        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        public unsafe void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, System.UInt16[] value)
        {
            if (value == null)
                return;

            this.MarshalFeature(context, ns, feature, value, 0, value.Length);
        }

        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature values.</param>
        /// <param name="offset">Start offset for feature values.</param>
        /// <param name="length">Length of feature values.</param>
        public unsafe void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, System.UInt16[] value, int offset, int length)
        {
            if (value == null)
                return;

            ulong i = 0;

            // support anchor feature
            if (feature.AddAnchor)
            {
                context.NamespaceBuilder.PreAllocate(value.Length + 1);

                context.NamespaceBuilder.AddFeature(ns.NamespaceHash, 1);
                i++;
            }
            else
                context.NamespaceBuilder.PreAllocate(value.Length);

            
            for (var j = offset;j<length;j++)
            {
                var v = value[j];

                
                context.NamespaceBuilder.AddFeature((ulong)(ns.NamespaceHash + i), (float)v);
                i++;
            }
            
            if (context.StringExample == null)
                return;

            string featureString;
            if (feature.Dictify && context.FastDictionary != null)
            {
                if (context.FastDictionary.TryGetValue(value, out featureString))
                {
                    context.AppendStringExample(feature.Dictify, featureString);
                    return;
                }
            }

            var featureBuilder = new StringBuilder();

            // support anchor feature
            i = 0;
            if (feature.AddAnchor)
            {
                featureBuilder.Append(" 0:1");
                i++;
            }

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            for (var j = offset;j<length;j++)
            {
                featureBuilder.AppendFormat(
                    CultureInfo.InvariantCulture,
                    format,
                    i,
                    value[j]);
                i++;
            }

            featureString = featureBuilder.ToString();

            if (feature.Dictify && context.FastDictionary != null)
                context.FastDictionary.Add(value, featureString);

            context.AppendStringExample(feature.Dictify, featureString);
        }

        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IEnumerable<System.UInt16> value)
        {
            if (value == null)
            {
                return;
            }

            ulong i = 0;

            // support anchor feature
            if (feature.AddAnchor)
            {
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash, 1);
                i++;
            }

            foreach (var v in value)
            {
                
                context.NamespaceBuilder.AddFeature((ulong)(ns.NamespaceHash + i), (float)v);
                i++;
            }

            if (context.StringExample == null)
                return;

            string featureString;
            if (feature.Dictify && context.FastDictionary != null)
            {
                if (context.FastDictionary.TryGetValue(value, out featureString))
                {
                    context.AppendStringExample(feature.Dictify, featureString);
                    return;
                }
            }

            var featureBuilder = new StringBuilder();

            // support anchor feature
            i = 0;
            if (feature.AddAnchor)
            {
                featureBuilder.Append(" 0:1");
                i++;
            }

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var v in value)
            {
                featureBuilder.AppendFormat(
                    CultureInfo.InvariantCulture,
                    format,
                    i,
                    v);
                i++;
            }

            featureString = featureBuilder.ToString();

            if (feature.Dictify && context.FastDictionary != null)
                context.FastDictionary.Add(value, featureString);

            context.AppendStringExample(feature.Dictify, featureString);
        }

                /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, PreHashedFeature feature, System.UInt32 value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            
            context.NamespaceBuilder.AddFeature(feature.FeatureHash, (float)value);

            context.AppendStringExample(
                feature.Dictify,
                " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}"),
                feature.Name,
                value);
        }

        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        public unsafe void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, System.UInt32[] value)
        {
            if (value == null)
                return;

            this.MarshalFeature(context, ns, feature, value, 0, value.Length);
        }

        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature values.</param>
        /// <param name="offset">Start offset for feature values.</param>
        /// <param name="length">Length of feature values.</param>
        public unsafe void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, System.UInt32[] value, int offset, int length)
        {
            if (value == null)
                return;

            ulong i = 0;

            // support anchor feature
            if (feature.AddAnchor)
            {
                context.NamespaceBuilder.PreAllocate(value.Length + 1);

                context.NamespaceBuilder.AddFeature(ns.NamespaceHash, 1);
                i++;
            }
            else
                context.NamespaceBuilder.PreAllocate(value.Length);

            
            for (var j = offset;j<length;j++)
            {
                var v = value[j];

                
                context.NamespaceBuilder.AddFeature((ulong)(ns.NamespaceHash + i), (float)v);
                i++;
            }
            
            if (context.StringExample == null)
                return;

            string featureString;
            if (feature.Dictify && context.FastDictionary != null)
            {
                if (context.FastDictionary.TryGetValue(value, out featureString))
                {
                    context.AppendStringExample(feature.Dictify, featureString);
                    return;
                }
            }

            var featureBuilder = new StringBuilder();

            // support anchor feature
            i = 0;
            if (feature.AddAnchor)
            {
                featureBuilder.Append(" 0:1");
                i++;
            }

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            for (var j = offset;j<length;j++)
            {
                featureBuilder.AppendFormat(
                    CultureInfo.InvariantCulture,
                    format,
                    i,
                    value[j]);
                i++;
            }

            featureString = featureBuilder.ToString();

            if (feature.Dictify && context.FastDictionary != null)
                context.FastDictionary.Add(value, featureString);

            context.AppendStringExample(feature.Dictify, featureString);
        }

        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IEnumerable<System.UInt32> value)
        {
            if (value == null)
            {
                return;
            }

            ulong i = 0;

            // support anchor feature
            if (feature.AddAnchor)
            {
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash, 1);
                i++;
            }

            foreach (var v in value)
            {
                
                context.NamespaceBuilder.AddFeature((ulong)(ns.NamespaceHash + i), (float)v);
                i++;
            }

            if (context.StringExample == null)
                return;

            string featureString;
            if (feature.Dictify && context.FastDictionary != null)
            {
                if (context.FastDictionary.TryGetValue(value, out featureString))
                {
                    context.AppendStringExample(feature.Dictify, featureString);
                    return;
                }
            }

            var featureBuilder = new StringBuilder();

            // support anchor feature
            i = 0;
            if (feature.AddAnchor)
            {
                featureBuilder.Append(" 0:1");
                i++;
            }

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var v in value)
            {
                featureBuilder.AppendFormat(
                    CultureInfo.InvariantCulture,
                    format,
                    i,
                    v);
                i++;
            }

            featureString = featureBuilder.ToString();

            if (feature.Dictify && context.FastDictionary != null)
                context.FastDictionary.Add(value, featureString);

            context.AppendStringExample(feature.Dictify, featureString);
        }

                /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, PreHashedFeature feature, System.Single value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            
            context.NamespaceBuilder.AddFeature(feature.FeatureHash, (float)value);

            context.AppendStringExample(
                feature.Dictify,
                " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}"),
                feature.Name,
                value);
        }

        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        public unsafe void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, System.Single[] value)
        {
            if (value == null)
                return;

            this.MarshalFeature(context, ns, feature, value, 0, value.Length);
        }

        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature values.</param>
        /// <param name="offset">Start offset for feature values.</param>
        /// <param name="length">Length of feature values.</param>
        public unsafe void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, System.Single[] value, int offset, int length)
        {
            if (value == null)
                return;

            ulong i = 0;

            // support anchor feature
            if (feature.AddAnchor)
            {
                context.NamespaceBuilder.PreAllocate(value.Length + 1);

                context.NamespaceBuilder.AddFeature(ns.NamespaceHash, 1);
                i++;
            }
            else
                context.NamespaceBuilder.PreAllocate(value.Length);

            
            fixed (float* begin = value)
            {
                var temp = begin + offset;
                context.NamespaceBuilder.AddFeaturesUnchecked((ulong)(ns.NamespaceHash + i), temp, temp + length);
            }

            
            if (context.StringExample == null)
                return;

            string featureString;
            if (feature.Dictify && context.FastDictionary != null)
            {
                if (context.FastDictionary.TryGetValue(value, out featureString))
                {
                    context.AppendStringExample(feature.Dictify, featureString);
                    return;
                }
            }

            var featureBuilder = new StringBuilder();

            // support anchor feature
            i = 0;
            if (feature.AddAnchor)
            {
                featureBuilder.Append(" 0:1");
                i++;
            }

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            for (var j = offset;j<length;j++)
            {
                featureBuilder.AppendFormat(
                    CultureInfo.InvariantCulture,
                    format,
                    i,
                    value[j]);
                i++;
            }

            featureString = featureBuilder.ToString();

            if (feature.Dictify && context.FastDictionary != null)
                context.FastDictionary.Add(value, featureString);

            context.AppendStringExample(feature.Dictify, featureString);
        }

        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IEnumerable<System.Single> value)
        {
            if (value == null)
            {
                return;
            }

            ulong i = 0;

            // support anchor feature
            if (feature.AddAnchor)
            {
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash, 1);
                i++;
            }

            foreach (var v in value)
            {
                
                context.NamespaceBuilder.AddFeature((ulong)(ns.NamespaceHash + i), (float)v);
                i++;
            }

            if (context.StringExample == null)
                return;

            string featureString;
            if (feature.Dictify && context.FastDictionary != null)
            {
                if (context.FastDictionary.TryGetValue(value, out featureString))
                {
                    context.AppendStringExample(feature.Dictify, featureString);
                    return;
                }
            }

            var featureBuilder = new StringBuilder();

            // support anchor feature
            i = 0;
            if (feature.AddAnchor)
            {
                featureBuilder.Append(" 0:1");
                i++;
            }

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var v in value)
            {
                featureBuilder.AppendFormat(
                    CultureInfo.InvariantCulture,
                    format,
                    i,
                    v);
                i++;
            }

            featureString = featureBuilder.ToString();

            if (feature.Dictify && context.FastDictionary != null)
                context.FastDictionary.Add(value, featureString);

            context.AppendStringExample(feature.Dictify, featureString);
        }

                /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, PreHashedFeature feature, System.Int64 value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

                        #if DEBUG
            if (value > float.MaxValue || value < float.MinValue)
            {
                Trace.TraceWarning("Precision lost for feature value: " + value);
            }
            #endif
            
            context.NamespaceBuilder.AddFeature(feature.FeatureHash, (float)value);

            context.AppendStringExample(
                feature.Dictify,
                " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}"),
                feature.Name,
                value);
        }

        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        public unsafe void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, System.Int64[] value)
        {
            if (value == null)
                return;

            this.MarshalFeature(context, ns, feature, value, 0, value.Length);
        }

        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature values.</param>
        /// <param name="offset">Start offset for feature values.</param>
        /// <param name="length">Length of feature values.</param>
        public unsafe void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, System.Int64[] value, int offset, int length)
        {
            if (value == null)
                return;

            ulong i = 0;

            // support anchor feature
            if (feature.AddAnchor)
            {
                context.NamespaceBuilder.PreAllocate(value.Length + 1);

                context.NamespaceBuilder.AddFeature(ns.NamespaceHash, 1);
                i++;
            }
            else
                context.NamespaceBuilder.PreAllocate(value.Length);

            
            for (var j = offset;j<length;j++)
            {
                var v = value[j];

                                #if DEBUG
                if (v > float.MaxValue || v < float.MinValue)
                {
                    Trace.TraceWarning("Precision lost for feature value: " + v);
                }
                #endif
                
                context.NamespaceBuilder.AddFeature((ulong)(ns.NamespaceHash + i), (float)v);
                i++;
            }
            
            if (context.StringExample == null)
                return;

            string featureString;
            if (feature.Dictify && context.FastDictionary != null)
            {
                if (context.FastDictionary.TryGetValue(value, out featureString))
                {
                    context.AppendStringExample(feature.Dictify, featureString);
                    return;
                }
            }

            var featureBuilder = new StringBuilder();

            // support anchor feature
            i = 0;
            if (feature.AddAnchor)
            {
                featureBuilder.Append(" 0:1");
                i++;
            }

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            for (var j = offset;j<length;j++)
            {
                featureBuilder.AppendFormat(
                    CultureInfo.InvariantCulture,
                    format,
                    i,
                    value[j]);
                i++;
            }

            featureString = featureBuilder.ToString();

            if (feature.Dictify && context.FastDictionary != null)
                context.FastDictionary.Add(value, featureString);

            context.AppendStringExample(feature.Dictify, featureString);
        }

        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IEnumerable<System.Int64> value)
        {
            if (value == null)
            {
                return;
            }

            ulong i = 0;

            // support anchor feature
            if (feature.AddAnchor)
            {
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash, 1);
                i++;
            }

            foreach (var v in value)
            {
                                #if DEBUG
                if (v > float.MaxValue || v < float.MinValue)
                {
                    Trace.TraceWarning("Precision lost for feature value: " + v);
                }
                #endif
                
                context.NamespaceBuilder.AddFeature((ulong)(ns.NamespaceHash + i), (float)v);
                i++;
            }

            if (context.StringExample == null)
                return;

            string featureString;
            if (feature.Dictify && context.FastDictionary != null)
            {
                if (context.FastDictionary.TryGetValue(value, out featureString))
                {
                    context.AppendStringExample(feature.Dictify, featureString);
                    return;
                }
            }

            var featureBuilder = new StringBuilder();

            // support anchor feature
            i = 0;
            if (feature.AddAnchor)
            {
                featureBuilder.Append(" 0:1");
                i++;
            }

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var v in value)
            {
                featureBuilder.AppendFormat(
                    CultureInfo.InvariantCulture,
                    format,
                    i,
                    v);
                i++;
            }

            featureString = featureBuilder.ToString();

            if (feature.Dictify && context.FastDictionary != null)
                context.FastDictionary.Add(value, featureString);

            context.AppendStringExample(feature.Dictify, featureString);
        }

                /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, PreHashedFeature feature, System.UInt64 value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

                        #if DEBUG
            if (value > float.MaxValue || value < float.MinValue)
            {
                Trace.TraceWarning("Precision lost for feature value: " + value);
            }
            #endif
            
            context.NamespaceBuilder.AddFeature(feature.FeatureHash, (float)value);

            context.AppendStringExample(
                feature.Dictify,
                " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}"),
                feature.Name,
                value);
        }

        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        public unsafe void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, System.UInt64[] value)
        {
            if (value == null)
                return;

            this.MarshalFeature(context, ns, feature, value, 0, value.Length);
        }

        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature values.</param>
        /// <param name="offset">Start offset for feature values.</param>
        /// <param name="length">Length of feature values.</param>
        public unsafe void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, System.UInt64[] value, int offset, int length)
        {
            if (value == null)
                return;

            ulong i = 0;

            // support anchor feature
            if (feature.AddAnchor)
            {
                context.NamespaceBuilder.PreAllocate(value.Length + 1);

                context.NamespaceBuilder.AddFeature(ns.NamespaceHash, 1);
                i++;
            }
            else
                context.NamespaceBuilder.PreAllocate(value.Length);

            
            for (var j = offset;j<length;j++)
            {
                var v = value[j];

                                #if DEBUG
                if (v > float.MaxValue || v < float.MinValue)
                {
                    Trace.TraceWarning("Precision lost for feature value: " + v);
                }
                #endif
                
                context.NamespaceBuilder.AddFeature((ulong)(ns.NamespaceHash + i), (float)v);
                i++;
            }
            
            if (context.StringExample == null)
                return;

            string featureString;
            if (feature.Dictify && context.FastDictionary != null)
            {
                if (context.FastDictionary.TryGetValue(value, out featureString))
                {
                    context.AppendStringExample(feature.Dictify, featureString);
                    return;
                }
            }

            var featureBuilder = new StringBuilder();

            // support anchor feature
            i = 0;
            if (feature.AddAnchor)
            {
                featureBuilder.Append(" 0:1");
                i++;
            }

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            for (var j = offset;j<length;j++)
            {
                featureBuilder.AppendFormat(
                    CultureInfo.InvariantCulture,
                    format,
                    i,
                    value[j]);
                i++;
            }

            featureString = featureBuilder.ToString();

            if (feature.Dictify && context.FastDictionary != null)
                context.FastDictionary.Add(value, featureString);

            context.AppendStringExample(feature.Dictify, featureString);
        }

        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IEnumerable<System.UInt64> value)
        {
            if (value == null)
            {
                return;
            }

            ulong i = 0;

            // support anchor feature
            if (feature.AddAnchor)
            {
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash, 1);
                i++;
            }

            foreach (var v in value)
            {
                                #if DEBUG
                if (v > float.MaxValue || v < float.MinValue)
                {
                    Trace.TraceWarning("Precision lost for feature value: " + v);
                }
                #endif
                
                context.NamespaceBuilder.AddFeature((ulong)(ns.NamespaceHash + i), (float)v);
                i++;
            }

            if (context.StringExample == null)
                return;

            string featureString;
            if (feature.Dictify && context.FastDictionary != null)
            {
                if (context.FastDictionary.TryGetValue(value, out featureString))
                {
                    context.AppendStringExample(feature.Dictify, featureString);
                    return;
                }
            }

            var featureBuilder = new StringBuilder();

            // support anchor feature
            i = 0;
            if (feature.AddAnchor)
            {
                featureBuilder.Append(" 0:1");
                i++;
            }

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var v in value)
            {
                featureBuilder.AppendFormat(
                    CultureInfo.InvariantCulture,
                    format,
                    i,
                    v);
                i++;
            }

            featureString = featureBuilder.ToString();

            if (feature.Dictify && context.FastDictionary != null)
                context.FastDictionary.Add(value, featureString);

            context.AppendStringExample(feature.Dictify, featureString);
        }

                /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, PreHashedFeature feature, System.Double value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

                        #if DEBUG
            if (value > float.MaxValue || value < float.MinValue)
            {
                Trace.TraceWarning("Precision lost for feature value: " + value);
            }
            #endif
            
            context.NamespaceBuilder.AddFeature(feature.FeatureHash, (float)value);

            context.AppendStringExample(
                feature.Dictify,
                " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}"),
                feature.Name,
                value);
        }

        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        public unsafe void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, System.Double[] value)
        {
            if (value == null)
                return;

            this.MarshalFeature(context, ns, feature, value, 0, value.Length);
        }

        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature values.</param>
        /// <param name="offset">Start offset for feature values.</param>
        /// <param name="length">Length of feature values.</param>
        public unsafe void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, System.Double[] value, int offset, int length)
        {
            if (value == null)
                return;

            ulong i = 0;

            // support anchor feature
            if (feature.AddAnchor)
            {
                context.NamespaceBuilder.PreAllocate(value.Length + 1);

                context.NamespaceBuilder.AddFeature(ns.NamespaceHash, 1);
                i++;
            }
            else
                context.NamespaceBuilder.PreAllocate(value.Length);

            
            for (var j = offset;j<length;j++)
            {
                var v = value[j];

                                #if DEBUG
                if (v > float.MaxValue || v < float.MinValue)
                {
                    Trace.TraceWarning("Precision lost for feature value: " + v);
                }
                #endif
                
                context.NamespaceBuilder.AddFeature((ulong)(ns.NamespaceHash + i), (float)v);
                i++;
            }
            
            if (context.StringExample == null)
                return;

            string featureString;
            if (feature.Dictify && context.FastDictionary != null)
            {
                if (context.FastDictionary.TryGetValue(value, out featureString))
                {
                    context.AppendStringExample(feature.Dictify, featureString);
                    return;
                }
            }

            var featureBuilder = new StringBuilder();

            // support anchor feature
            i = 0;
            if (feature.AddAnchor)
            {
                featureBuilder.Append(" 0:1");
                i++;
            }

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            for (var j = offset;j<length;j++)
            {
                featureBuilder.AppendFormat(
                    CultureInfo.InvariantCulture,
                    format,
                    i,
                    value[j]);
                i++;
            }

            featureString = featureBuilder.ToString();

            if (feature.Dictify && context.FastDictionary != null)
                context.FastDictionary.Add(value, featureString);

            context.AppendStringExample(feature.Dictify, featureString);
        }

        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IEnumerable<System.Double> value)
        {
            if (value == null)
            {
                return;
            }

            ulong i = 0;

            // support anchor feature
            if (feature.AddAnchor)
            {
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash, 1);
                i++;
            }

            foreach (var v in value)
            {
                                #if DEBUG
                if (v > float.MaxValue || v < float.MinValue)
                {
                    Trace.TraceWarning("Precision lost for feature value: " + v);
                }
                #endif
                
                context.NamespaceBuilder.AddFeature((ulong)(ns.NamespaceHash + i), (float)v);
                i++;
            }

            if (context.StringExample == null)
                return;

            string featureString;
            if (feature.Dictify && context.FastDictionary != null)
            {
                if (context.FastDictionary.TryGetValue(value, out featureString))
                {
                    context.AppendStringExample(feature.Dictify, featureString);
                    return;
                }
            }

            var featureBuilder = new StringBuilder();

            // support anchor feature
            i = 0;
            if (feature.AddAnchor)
            {
                featureBuilder.Append(" 0:1");
                i++;
            }

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var v in value)
            {
                featureBuilder.AppendFormat(
                    CultureInfo.InvariantCulture,
                    format,
                    i,
                    v);
                i++;
            }

            featureString = featureBuilder.ToString();

            if (feature.Dictify && context.FastDictionary != null)
                context.FastDictionary.Add(value, featureString);

            context.AppendStringExample(feature.Dictify, featureString);
        }

        
        
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.Byte, System.Byte> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.Byte, System.SByte> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.Byte, System.Int16> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.Byte, System.Int32> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.Byte, System.UInt16> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.Byte, System.UInt32> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.Byte, System.Single> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.Byte, System.Int64> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                                #if DEBUG
                if (kvp.Value > float.MaxValue || kvp.Value < float.MinValue)
                {
                    Trace.TraceWarning("Precision lost for feature value: " + kvp.Value);
                }
                #endif
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.Byte, System.UInt64> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                                #if DEBUG
                if (kvp.Value > float.MaxValue || kvp.Value < float.MinValue)
                {
                    Trace.TraceWarning("Precision lost for feature value: " + kvp.Value);
                }
                #endif
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.Byte, System.Double> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                                #if DEBUG
                if (kvp.Value > float.MaxValue || kvp.Value < float.MinValue)
                {
                    Trace.TraceWarning("Precision lost for feature value: " + kvp.Value);
                }
                #endif
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.SByte, System.Byte> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.SByte, System.SByte> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.SByte, System.Int16> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.SByte, System.Int32> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.SByte, System.UInt16> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.SByte, System.UInt32> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.SByte, System.Single> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.SByte, System.Int64> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                                #if DEBUG
                if (kvp.Value > float.MaxValue || kvp.Value < float.MinValue)
                {
                    Trace.TraceWarning("Precision lost for feature value: " + kvp.Value);
                }
                #endif
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.SByte, System.UInt64> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                                #if DEBUG
                if (kvp.Value > float.MaxValue || kvp.Value < float.MinValue)
                {
                    Trace.TraceWarning("Precision lost for feature value: " + kvp.Value);
                }
                #endif
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.SByte, System.Double> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                                #if DEBUG
                if (kvp.Value > float.MaxValue || kvp.Value < float.MinValue)
                {
                    Trace.TraceWarning("Precision lost for feature value: " + kvp.Value);
                }
                #endif
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.Int16, System.Byte> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.Int16, System.SByte> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.Int16, System.Int16> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.Int16, System.Int32> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.Int16, System.UInt16> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.Int16, System.UInt32> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.Int16, System.Single> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.Int16, System.Int64> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                                #if DEBUG
                if (kvp.Value > float.MaxValue || kvp.Value < float.MinValue)
                {
                    Trace.TraceWarning("Precision lost for feature value: " + kvp.Value);
                }
                #endif
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.Int16, System.UInt64> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                                #if DEBUG
                if (kvp.Value > float.MaxValue || kvp.Value < float.MinValue)
                {
                    Trace.TraceWarning("Precision lost for feature value: " + kvp.Value);
                }
                #endif
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.Int16, System.Double> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                                #if DEBUG
                if (kvp.Value > float.MaxValue || kvp.Value < float.MinValue)
                {
                    Trace.TraceWarning("Precision lost for feature value: " + kvp.Value);
                }
                #endif
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.Int32, System.Byte> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.Int32, System.SByte> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.Int32, System.Int16> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.Int32, System.Int32> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.Int32, System.UInt16> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.Int32, System.UInt32> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.Int32, System.Single> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.Int32, System.Int64> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                                #if DEBUG
                if (kvp.Value > float.MaxValue || kvp.Value < float.MinValue)
                {
                    Trace.TraceWarning("Precision lost for feature value: " + kvp.Value);
                }
                #endif
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.Int32, System.UInt64> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                                #if DEBUG
                if (kvp.Value > float.MaxValue || kvp.Value < float.MinValue)
                {
                    Trace.TraceWarning("Precision lost for feature value: " + kvp.Value);
                }
                #endif
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.Int32, System.Double> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                                #if DEBUG
                if (kvp.Value > float.MaxValue || kvp.Value < float.MinValue)
                {
                    Trace.TraceWarning("Precision lost for feature value: " + kvp.Value);
                }
                #endif
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.UInt16, System.Byte> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.UInt16, System.SByte> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.UInt16, System.Int16> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.UInt16, System.Int32> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.UInt16, System.UInt16> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.UInt16, System.UInt32> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.UInt16, System.Single> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.UInt16, System.Int64> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                                #if DEBUG
                if (kvp.Value > float.MaxValue || kvp.Value < float.MinValue)
                {
                    Trace.TraceWarning("Precision lost for feature value: " + kvp.Value);
                }
                #endif
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.UInt16, System.UInt64> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                                #if DEBUG
                if (kvp.Value > float.MaxValue || kvp.Value < float.MinValue)
                {
                    Trace.TraceWarning("Precision lost for feature value: " + kvp.Value);
                }
                #endif
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<System.UInt16, System.Double> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                                #if DEBUG
                if (kvp.Value > float.MaxValue || kvp.Value < float.MinValue)
                {
                    Trace.TraceWarning("Precision lost for feature value: " + kvp.Value);
                }
                #endif
                
                context.NamespaceBuilder.AddFeature(ns.NamespaceHash + (ulong)kvp.Key, (float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }
        
        
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<string, System.Byte> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(
					context.VW.HashFeature(kvp.Key, ns.NamespaceHash),
					(float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }

		/// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public int MarshalNamespace(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<string, System.Byte[]> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

			if (value == null)
                return 0;

			int featureCount = 0;
            foreach (var kvp in value)
			{
				var perKeyNs = kvp.Key == null ? ns : new Namespace(context.VW, kvp.Key);

				try
				{
					// the namespace is only added on dispose, to be able to check if at least a single feature has been added
					context.NamespaceBuilder = context.ExampleBuilder.AddNamespace(perKeyNs.FeatureGroup);

					var position = 0;
					var stringExample = context.StringExample;
					if (context.StringExample != null)
					{
						position = stringExample.Append(perKeyNs.NamespaceString).Length;
					}

					this.MarshalFeature(context, perKeyNs, feature, kvp.Value);

					if (context.StringExample != null)
					{
						if (position == stringExample.Length)
						{
							// no features added, remove namespace
							stringExample.Length = position - ns.NamespaceString.Length;
						}
					}

					featureCount += (int)context.NamespaceBuilder.FeatureCount;
				}
				finally
				{
					if (context.NamespaceBuilder != null)
					{
						context.NamespaceBuilder.Dispose();
						context.NamespaceBuilder = null;
					}
				}
            }

			return featureCount;
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<string, System.SByte> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(
					context.VW.HashFeature(kvp.Key, ns.NamespaceHash),
					(float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }

		/// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public int MarshalNamespace(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<string, System.SByte[]> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

			if (value == null)
                return 0;

			int featureCount = 0;
            foreach (var kvp in value)
			{
				var perKeyNs = kvp.Key == null ? ns : new Namespace(context.VW, kvp.Key);

				try
				{
					// the namespace is only added on dispose, to be able to check if at least a single feature has been added
					context.NamespaceBuilder = context.ExampleBuilder.AddNamespace(perKeyNs.FeatureGroup);

					var position = 0;
					var stringExample = context.StringExample;
					if (context.StringExample != null)
					{
						position = stringExample.Append(perKeyNs.NamespaceString).Length;
					}

					this.MarshalFeature(context, perKeyNs, feature, kvp.Value);

					if (context.StringExample != null)
					{
						if (position == stringExample.Length)
						{
							// no features added, remove namespace
							stringExample.Length = position - ns.NamespaceString.Length;
						}
					}

					featureCount += (int)context.NamespaceBuilder.FeatureCount;
				}
				finally
				{
					if (context.NamespaceBuilder != null)
					{
						context.NamespaceBuilder.Dispose();
						context.NamespaceBuilder = null;
					}
				}
            }

			return featureCount;
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<string, System.Int16> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(
					context.VW.HashFeature(kvp.Key, ns.NamespaceHash),
					(float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }

		/// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public int MarshalNamespace(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<string, System.Int16[]> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

			if (value == null)
                return 0;

			int featureCount = 0;
            foreach (var kvp in value)
			{
				var perKeyNs = kvp.Key == null ? ns : new Namespace(context.VW, kvp.Key);

				try
				{
					// the namespace is only added on dispose, to be able to check if at least a single feature has been added
					context.NamespaceBuilder = context.ExampleBuilder.AddNamespace(perKeyNs.FeatureGroup);

					var position = 0;
					var stringExample = context.StringExample;
					if (context.StringExample != null)
					{
						position = stringExample.Append(perKeyNs.NamespaceString).Length;
					}

					this.MarshalFeature(context, perKeyNs, feature, kvp.Value);

					if (context.StringExample != null)
					{
						if (position == stringExample.Length)
						{
							// no features added, remove namespace
							stringExample.Length = position - ns.NamespaceString.Length;
						}
					}

					featureCount += (int)context.NamespaceBuilder.FeatureCount;
				}
				finally
				{
					if (context.NamespaceBuilder != null)
					{
						context.NamespaceBuilder.Dispose();
						context.NamespaceBuilder = null;
					}
				}
            }

			return featureCount;
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<string, System.Int32> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(
					context.VW.HashFeature(kvp.Key, ns.NamespaceHash),
					(float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }

		/// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public int MarshalNamespace(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<string, System.Int32[]> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

			if (value == null)
                return 0;

			int featureCount = 0;
            foreach (var kvp in value)
			{
				var perKeyNs = kvp.Key == null ? ns : new Namespace(context.VW, kvp.Key);

				try
				{
					// the namespace is only added on dispose, to be able to check if at least a single feature has been added
					context.NamespaceBuilder = context.ExampleBuilder.AddNamespace(perKeyNs.FeatureGroup);

					var position = 0;
					var stringExample = context.StringExample;
					if (context.StringExample != null)
					{
						position = stringExample.Append(perKeyNs.NamespaceString).Length;
					}

					this.MarshalFeature(context, perKeyNs, feature, kvp.Value);

					if (context.StringExample != null)
					{
						if (position == stringExample.Length)
						{
							// no features added, remove namespace
							stringExample.Length = position - ns.NamespaceString.Length;
						}
					}

					featureCount += (int)context.NamespaceBuilder.FeatureCount;
				}
				finally
				{
					if (context.NamespaceBuilder != null)
					{
						context.NamespaceBuilder.Dispose();
						context.NamespaceBuilder = null;
					}
				}
            }

			return featureCount;
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<string, System.UInt16> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(
					context.VW.HashFeature(kvp.Key, ns.NamespaceHash),
					(float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }

		/// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public int MarshalNamespace(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<string, System.UInt16[]> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

			if (value == null)
                return 0;

			int featureCount = 0;
            foreach (var kvp in value)
			{
				var perKeyNs = kvp.Key == null ? ns : new Namespace(context.VW, kvp.Key);

				try
				{
					// the namespace is only added on dispose, to be able to check if at least a single feature has been added
					context.NamespaceBuilder = context.ExampleBuilder.AddNamespace(perKeyNs.FeatureGroup);

					var position = 0;
					var stringExample = context.StringExample;
					if (context.StringExample != null)
					{
						position = stringExample.Append(perKeyNs.NamespaceString).Length;
					}

					this.MarshalFeature(context, perKeyNs, feature, kvp.Value);

					if (context.StringExample != null)
					{
						if (position == stringExample.Length)
						{
							// no features added, remove namespace
							stringExample.Length = position - ns.NamespaceString.Length;
						}
					}

					featureCount += (int)context.NamespaceBuilder.FeatureCount;
				}
				finally
				{
					if (context.NamespaceBuilder != null)
					{
						context.NamespaceBuilder.Dispose();
						context.NamespaceBuilder = null;
					}
				}
            }

			return featureCount;
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<string, System.UInt32> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(
					context.VW.HashFeature(kvp.Key, ns.NamespaceHash),
					(float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }

		/// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public int MarshalNamespace(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<string, System.UInt32[]> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

			if (value == null)
                return 0;

			int featureCount = 0;
            foreach (var kvp in value)
			{
				var perKeyNs = kvp.Key == null ? ns : new Namespace(context.VW, kvp.Key);

				try
				{
					// the namespace is only added on dispose, to be able to check if at least a single feature has been added
					context.NamespaceBuilder = context.ExampleBuilder.AddNamespace(perKeyNs.FeatureGroup);

					var position = 0;
					var stringExample = context.StringExample;
					if (context.StringExample != null)
					{
						position = stringExample.Append(perKeyNs.NamespaceString).Length;
					}

					this.MarshalFeature(context, perKeyNs, feature, kvp.Value);

					if (context.StringExample != null)
					{
						if (position == stringExample.Length)
						{
							// no features added, remove namespace
							stringExample.Length = position - ns.NamespaceString.Length;
						}
					}

					featureCount += (int)context.NamespaceBuilder.FeatureCount;
				}
				finally
				{
					if (context.NamespaceBuilder != null)
					{
						context.NamespaceBuilder.Dispose();
						context.NamespaceBuilder = null;
					}
				}
            }

			return featureCount;
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<string, System.Single> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                
                context.NamespaceBuilder.AddFeature(
					context.VW.HashFeature(kvp.Key, ns.NamespaceHash),
					(float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }

		/// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public int MarshalNamespace(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<string, System.Single[]> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

			if (value == null)
                return 0;

			int featureCount = 0;
            foreach (var kvp in value)
			{
				var perKeyNs = kvp.Key == null ? ns : new Namespace(context.VW, kvp.Key);

				try
				{
					// the namespace is only added on dispose, to be able to check if at least a single feature has been added
					context.NamespaceBuilder = context.ExampleBuilder.AddNamespace(perKeyNs.FeatureGroup);

					var position = 0;
					var stringExample = context.StringExample;
					if (context.StringExample != null)
					{
						position = stringExample.Append(perKeyNs.NamespaceString).Length;
					}

					this.MarshalFeature(context, perKeyNs, feature, kvp.Value);

					if (context.StringExample != null)
					{
						if (position == stringExample.Length)
						{
							// no features added, remove namespace
							stringExample.Length = position - ns.NamespaceString.Length;
						}
					}

					featureCount += (int)context.NamespaceBuilder.FeatureCount;
				}
				finally
				{
					if (context.NamespaceBuilder != null)
					{
						context.NamespaceBuilder.Dispose();
						context.NamespaceBuilder = null;
					}
				}
            }

			return featureCount;
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<string, System.Int64> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                                #if DEBUG
                if (kvp.Value > float.MaxValue || kvp.Value < float.MinValue)
                {
                    Trace.TraceWarning("Precision lost for feature value: " + kvp.Value);
                }
                #endif
                
                context.NamespaceBuilder.AddFeature(
					context.VW.HashFeature(kvp.Key, ns.NamespaceHash),
					(float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }

		/// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public int MarshalNamespace(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<string, System.Int64[]> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

			if (value == null)
                return 0;

			int featureCount = 0;
            foreach (var kvp in value)
			{
				var perKeyNs = kvp.Key == null ? ns : new Namespace(context.VW, kvp.Key);

				try
				{
					// the namespace is only added on dispose, to be able to check if at least a single feature has been added
					context.NamespaceBuilder = context.ExampleBuilder.AddNamespace(perKeyNs.FeatureGroup);

					var position = 0;
					var stringExample = context.StringExample;
					if (context.StringExample != null)
					{
						position = stringExample.Append(perKeyNs.NamespaceString).Length;
					}

					this.MarshalFeature(context, perKeyNs, feature, kvp.Value);

					if (context.StringExample != null)
					{
						if (position == stringExample.Length)
						{
							// no features added, remove namespace
							stringExample.Length = position - ns.NamespaceString.Length;
						}
					}

					featureCount += (int)context.NamespaceBuilder.FeatureCount;
				}
				finally
				{
					if (context.NamespaceBuilder != null)
					{
						context.NamespaceBuilder.Dispose();
						context.NamespaceBuilder = null;
					}
				}
            }

			return featureCount;
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<string, System.UInt64> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                                #if DEBUG
                if (kvp.Value > float.MaxValue || kvp.Value < float.MinValue)
                {
                    Trace.TraceWarning("Precision lost for feature value: " + kvp.Value);
                }
                #endif
                
                context.NamespaceBuilder.AddFeature(
					context.VW.HashFeature(kvp.Key, ns.NamespaceHash),
					(float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }

		/// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public int MarshalNamespace(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<string, System.UInt64[]> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

			if (value == null)
                return 0;

			int featureCount = 0;
            foreach (var kvp in value)
			{
				var perKeyNs = kvp.Key == null ? ns : new Namespace(context.VW, kvp.Key);

				try
				{
					// the namespace is only added on dispose, to be able to check if at least a single feature has been added
					context.NamespaceBuilder = context.ExampleBuilder.AddNamespace(perKeyNs.FeatureGroup);

					var position = 0;
					var stringExample = context.StringExample;
					if (context.StringExample != null)
					{
						position = stringExample.Append(perKeyNs.NamespaceString).Length;
					}

					this.MarshalFeature(context, perKeyNs, feature, kvp.Value);

					if (context.StringExample != null)
					{
						if (position == stringExample.Length)
						{
							// no features added, remove namespace
							stringExample.Length = position - ns.NamespaceString.Length;
						}
					}

					featureCount += (int)context.NamespaceBuilder.FeatureCount;
				}
				finally
				{
					if (context.NamespaceBuilder != null)
					{
						context.NamespaceBuilder.Dispose();
						context.NamespaceBuilder = null;
					}
				}
            }

			return featureCount;
        }
        
        /// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public void MarshalFeature(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<string, System.Double> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

            if (value == null)
                return;

            foreach (var kvp in value)
            {
                                #if DEBUG
                if (kvp.Value > float.MaxValue || kvp.Value < float.MinValue)
                {
                    Trace.TraceWarning("Precision lost for feature value: " + kvp.Value);
                }
                #endif
                
                context.NamespaceBuilder.AddFeature(
					context.VW.HashFeature(kvp.Key, ns.NamespaceHash),
					(float)kvp.Value);
            }

            if (context.StringExample == null)
                return;

            var format = " {0}:" + (context.VW.Settings.EnableStringFloatCompact ? "{1}" : "{1:E20}");
            foreach (var kvp in value)
            {
                // TODO: not sure if negative numbers will work
                context.AppendStringExample(
                    feature.Dictify,
                    format,
                    kvp.Key,
                    kvp.Value);
            }
        }

		/// <summary>
        /// Transfers feature data to native space.
        /// </summary>
        /// <param name="context">The marshalling context.</param>
        /// <param name="ns">The namespace description.</param>
        /// <param name="feature">The feature description.</param>
        /// <param name="value">The feature value.</param>
        [ContractVerification(false)]
        public int MarshalNamespace(VowpalWabbitMarshalContext context, Namespace ns, Feature feature, IDictionary<string, System.Double[]> value)
        {
            Contract.Requires(context != null);
            Contract.Requires(ns != null);
            Contract.Requires(feature != null);

			if (value == null)
                return 0;

			int featureCount = 0;
            foreach (var kvp in value)
			{
				var perKeyNs = kvp.Key == null ? ns : new Namespace(context.VW, kvp.Key);

				try
				{
					// the namespace is only added on dispose, to be able to check if at least a single feature has been added
					context.NamespaceBuilder = context.ExampleBuilder.AddNamespace(perKeyNs.FeatureGroup);

					var position = 0;
					var stringExample = context.StringExample;
					if (context.StringExample != null)
					{
						position = stringExample.Append(perKeyNs.NamespaceString).Length;
					}

					this.MarshalFeature(context, perKeyNs, feature, kvp.Value);

					if (context.StringExample != null)
					{
						if (position == stringExample.Length)
						{
							// no features added, remove namespace
							stringExample.Length = position - ns.NamespaceString.Length;
						}
					}

					featureCount += (int)context.NamespaceBuilder.FeatureCount;
				}
				finally
				{
					if (context.NamespaceBuilder != null)
					{
						context.NamespaceBuilder.Dispose();
						context.NamespaceBuilder = null;
					}
				}
            }

			return featureCount;
        }
            }
}
