#include "pch.h"
#include "mkl.h"

extern "C"  _declspec(dllexport)
void CubeInterpolate(MKL_INT nx, const double* x, MKL_INT ny, const double* y, const double* ders,
	const double* scoeff, MKL_INT nsite, const double* site, MKL_INT ndorder, const MKL_INT * dorder,
	double* results, MKL_INT nlim, const double* l_lims, const double* r_lims, double* int_results, int& ret, bool isUniform)
{
	try {
		int status;
		DFTaskPtr task;
		status = dfdNewTask1D(&task, nx, x, isUniform ? DF_UNIFORM_PARTITION : DF_NON_UNIFORM_PARTITION, ny, y, DF_MATRIX_STORAGE_ROWS);
		if (status != DF_STATUS_OK) { ret = -1; return; }
		status = dfdEditPPSpline1D(task, DF_PP_CUBIC, DF_PP_NATURAL, DF_BC_2ND_LEFT_DER | DF_BC_2ND_RIGHT_DER,
			ders, DF_NO_IC, NULL, scoeff, DF_NO_HINT);
		if (status != DF_STATUS_OK) { ret = -2; return; }
		status = dfdConstruct1D(task, DF_PP_SPLINE, DF_METHOD_STD);
		if (status != DF_STATUS_OK) { ret = -3; return; }
		status = dfdInterpolate1D(task, DF_INTERP, DF_METHOD_PP, nsite, site, isUniform ? DF_UNIFORM_PARTITION : DF_NON_UNIFORM_PARTITION,
			ndorder, dorder, NULL, results, DF_MATRIX_STORAGE_ROWS, NULL);
		if (status != DF_STATUS_OK) { ret = -4; return; }
		status = dfdIntegrate1D(task, DF_METHOD_PP, nlim, l_lims, DF_SORTED_DATA, r_lims,
			DF_SORTED_DATA, NULL, NULL, int_results, DF_MATRIX_STORAGE_ROWS);
		if (status != DF_STATUS_OK) { ret = -5; return; }
		status = dfDeleteTask(&task);
		if (status != DF_STATUS_OK) { ret = -6; return; }
		ret = 0;
	}
	catch (...) {
		ret = 1;
	}
}