﻿using Serenity.ComponentModel;
using System;
using System.Collections;

namespace Serenity.CodeGeneration
{
    public partial class ServerTypingsGenerator : ServerImportGeneratorBase
    {
        private void GenerateEnum(Type enumType)
        {
            var codeNamespace = GetNamespace(enumType);
            var enumKey = EnumMapper.GetEnumTypeKey(enumType);

            cw.Indented("export enum ");
            var identifier = MakeFriendlyName(enumType, codeNamespace);
            generatedTypes.Add((codeNamespace.IsEmptyOrNull() ? "" : codeNamespace + ".") + identifier);

            cw.InBrace(delegate
            {
                var names = Enum.GetNames(enumType);
                var values = (IList)Enum.GetValues(enumType);

                var inserted = 0;
                for (var i = 0; i < names.Length; i++)
                {
                    var name = names[i];

                    var member = enumType.GetMember(name);
                    if (member != null && member.Length > 0 &&
                        member[0].GetAttribute<IgnoreAttribute>(false) != null)
                        continue;

                    if (inserted > 0)
                        sb.AppendLine(",");

                    cw.Indented(name);
                    sb.Append(" = ");
                    sb.Append(Convert.ToInt32(values[i]));
                    inserted++;
                }

                sb.AppendLine();
            });

            cw.Indented("Serenity.Decorators.registerEnum(");
            sb.Append(enumType.Name);
            sb.Append(", '");
            sb.Append(enumKey);
            sb.AppendLine("');");
        }
    }
}