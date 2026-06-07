using Data.Models;
using WebApi.Models.Responses;

namespace WebApi.Mapping;
// создание списка работ необходимые провести у биоматериала
public static class WorksheetMapper
{
    public static List<WorksheetRowDto> ToWorksheetRows(IEnumerable<TripodBarcodeMaterial> items)
    {
        return items.Select(ToWorksheetRow).ToList();
    }

    public static WorksheetRowDto ToWorksheetRow(TripodBarcodeMaterial item)
    {
        var bm = item.BarcodeMaterial;
        var kind = bm.Material?.MaterialName
                   ?? bm.AnalysisDep.AnalysisDepName;

        var analyses = bm.BarcodeAnalysises.Count > 0
            ? string.Join("; ", bm.BarcodeAnalysises.Select(ba => ba.Analysis.AnalysisName))
            : "—";

        return new WorksheetRowDto
        {
            BiomaterialBarcode = bm.BarcodeMatId.ToString("0"),
            Kind = kind,
            Analyses = analyses
        };
    }
}