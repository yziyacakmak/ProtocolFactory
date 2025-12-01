using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ProtocolFactory.Analyzers.Calculation;
using ProtocolFactory.Core.Attributes;
using ProtocolFactory.Core.Math;
using ProtocolFactory.Core.Models;

namespace ProtocolFactory.Analyzers;

//[Generator] //unused
public class ProtocolClassStructGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var protocolClasses = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (s, _) => s is ClassDeclarationSyntax c && c.AttributeLists.Count > 0,
                transform: static (ctx, cancellationToken) => GetProtocolClassInfo(ctx)
            )
            .Where(static info => info is not null)
            .Select(static (info, _) => info!);
        context.RegisterSourceOutput(protocolClasses,
            static (spc, classInfo) => Execute(spc, classInfo));
    }

    private static ClassInfo? GetProtocolClassInfo(GeneratorSyntaxContext context)
    {
        var classDeclaration = (ClassDeclarationSyntax)context.Node;
        var classSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol;

        if (classSymbol is null ||
            !classDeclaration.Modifiers.Any(mod => mod.IsKind(Microsoft.CodeAnalysis.CSharp.SyntaxKind.PartialKeyword)))
        {
            return null;
        }
        
        var hasProtocolAttribute = classSymbol.GetAttributes()
            .Any(attr => attr.AttributeClass?.Name == nameof(ProtocolAttribute));

        if (!hasProtocolAttribute)
        {
            return null;
        }
        
        var totalBitLength = 0;
        var fields = new List<PropertyInfo>();

        foreach (var member in classSymbol.GetMembers().OfType<IPropertySymbol>())
        {
            var fieldAttribute = member.GetAttributes()
                .FirstOrDefault(attr => attr.AttributeClass?.Name == nameof(ProtocolFieldAttribute));
            
            if (fieldAttribute is not null)
            {
                if (fieldAttribute.ConstructorArguments.Length == 3)
                {
                    var startBit = (int)fieldAttribute.ConstructorArguments[0].Value!;
                    var length = (int)fieldAttribute.ConstructorArguments[1].Value!;
                    var endianArgument = fieldAttribute.ConstructorArguments[2];

                    totalBitLength += length;
                    var enumIntValue = (int)endianArgument.Value!;
                    var endianStringName = enumIntValue == 0 ? "Little" : "Big";
                    var endianValue = (Endianness)enumIntValue;

                    var mask = Numerics.MaskCalculation(startBit, length);
                    var shift = Numerics.ShiftAmount(startBit, length);
                    var lsb = Numerics.MsbToLsbBigEndian(startBit, length);
                    var lengthAsByteHolder = (lsb / 8) - (startBit / 8) + 1;


                    fields.Add(new PropertyInfo(
                        Name: member.Name,
                        Type: member.Type.ToDisplayString(),
                        StartBit: startBit,
                        Length: length,
                        Endian: endianStringName,
                        Mask: mask, 
                        Shift: shift,
                        LengthAsByte: lengthAsByteHolder
                    ));
                }
            }
        }

        // 3. ClassInfo'yu Hesaplanan Verilerle Döndür
        return new ClassInfo(
            Name: classSymbol.Name,
            Namespace: classSymbol.ContainingNamespace.ToDisplayString(),
            Accessibility: classSymbol.DeclaredAccessibility.ToString().ToLower(),
            Fields: fields,
            TotalBitLength: totalBitLength,
            // Toplam Bayt Uzunluğu: (totalBitLength + 7) / 8
            TotalByteLength: (totalBitLength + 7) / 8
        );
    }

    private static void Execute(SourceProductionContext context, ClassInfo classInfo)
    {
        var sb = new System.Text.StringBuilder();
        var structName = $"{classInfo.Name}Value";
        var orderedFields = classInfo.Fields.OrderBy(f => f.StartBit).ToList();

        // ... (using ifadeleri) ...
        sb.AppendLine($"using System;");
        sb.AppendLine($"using ProtocolFactory.Core.Models;");
        sb.AppendLine($"using ProtocolFactory.Core.Contracts;");

        sb.AppendLine($"namespace {classInfo.Namespace}");
        sb.AppendLine($"{{");
        sb.AppendLine($"    {classInfo.Accessibility} readonly struct {structName} : IProtocolValue<{structName}>");
        sb.AppendLine($"    {{");

        // Dizi Değerlerini C# kodu olarak oluşturma
        var startBitsArray = $"new int[] {{ {string.Join(", ", orderedFields.Select(f => f.StartBit))} }}";
        var lengthsArray = $"new int[] {{ {string.Join(", ", orderedFields.Select(f => f.Length))} }}";

        // INT MASKELERİNİN OLUŞTURULMASI: ulong değerlerini int'e dönüştürerek
        var masksArrayInt = $"new int[] {{ {string.Join(", ", orderedFields.Select(f => $"(int)0x{f.Mask:X}"))} }}";
        var lengthAsByte = $"new int[] {{ {string.Join(", ", orderedFields.Select(f => f.LengthAsByte))} }}";
        var shiftsArray = $"new int[] {{ {string.Join(", ", orderedFields.Select(f => f.Shift))} }}";
        var endiansArray =
            $"new Endianness[] {{ {string.Join(", ", orderedFields.Select(f => $"Endianness.{f.Endian}"))} }}";

        // Arayüz Implementasyonları (Propertyler)
        sb.AppendLine($"        public int Length => {classInfo.TotalByteLength};");
        sb.AppendLine($"        public int FieldCount => {orderedFields.Count};");

        // 3. Dizi verilerini readonly static alanlar olarak tanımla
        sb.AppendLine($"");
        sb.AppendLine($"        // *** IProtocolValue Meta Verileri (ReadOnly Statics) ***");
        sb.AppendLine($"        private static readonly int[] _startBits = {startBitsArray};");
        sb.AppendLine($"        private static readonly int[] _lengths = {lengthsArray};");

        // ⚠️ MASKS UYUMU: Arayüz int[] istediği için yeni int[] alanı tanımlanır.
        sb.AppendLine($"        private static readonly int[] _masksInt = {masksArrayInt};");
        sb.AppendLine($"        private static readonly int[] _lengthAsByte = {lengthAsByte};");

        sb.AppendLine($"        private static readonly int[] _shifts = {shiftsArray};");
        sb.AppendLine($"        private static readonly Endianness[] _endians = {endiansArray};");

        // 4. Arayüz Property'lerini bu alanlara yönlendir
        sb.AppendLine($"        public int[] StartBits => _startBits;");
        sb.AppendLine($"        public int[] Lengths => _lengths;");

        // ⚠️ MASKS UYUMU: int[] tipini uygulayan alan kullanılır.
        sb.AppendLine($"        public int[] Masks => _masksInt;");
        sb.AppendLine($"        public int[] LengthAsByte => _lengthAsByte;");

        sb.AppendLine($"        public int[] Shifts => _shifts;");
        sb.AppendLine($"        public Endianness[] Endians => _endians;");

        // ... (Diğer struct gövdesi ve metotları) ...

        sb.AppendLine($"    }}");
        sb.AppendLine($"}}");

        context.AddSource($"{structName}.g.cs", sb.ToString());
    }

    public record PropertyInfo(
        string Name,
        string Type,
        int StartBit,
        int Length,
        string Endian,
        // Yeni hesaplanan değerler
        int Mask, // Maske (örneğin 0b1111)
        int Shift, // Kaydırma miktarı (ShiftAmount)
        int LengthAsByte // Özelliğin bayt cinsinden uzunluğu (genellikle Math.Ceiling(Length / 8.0))
    );

    public record ClassInfo(
        string Name,
        string Namespace,
        string Accessibility,
        List<PropertyInfo> Fields,
        int TotalBitLength, // Protokolün toplam bit uzunluğu
        int TotalByteLength // Protokolün toplam bayt uzunluğu (Math.Ceiling(TotalBitLength / 8.0))
    );
}