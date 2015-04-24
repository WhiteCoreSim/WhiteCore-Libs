///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 *	OPCODE - Optimized Collision Detection
 *	Copyright (C) 2001 Pierre Terdiman
 *	Homepage: http://www.codercorner.com/Opcode.htm
 */
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/**
 *	Contains code for box pruning.
 *	\file		IceBoxPruning.h
 *	\author		Pierre Terdiman
 *	\date		January, 29, 2000
 */
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Include Guard
#ifndef __OPC_BOXPRUNING_H__
#define __OPC_BOXPRUNING_H__

	// Optimized versions
	FUNCTION OPCODE_API bool CompleteBoxPruning(udword nb, const AABB** array, Pairs& pairs, const Axes& axes);
	FUNCTION OPCODE_API bool BipartiteBoxPruning(udword nb0, const AABB** array0, udword nb1, const AABB** array1, Pairs& pairs, const Axes& axes);

	// Brute-force versions
	FUNCTION OPCODE_API bool BruteForceCompleteBoxTest(udword nb, const AABB** array, Pairs& pairs);
	FUNCTION OPCODE_API bool BruteForceBipartiteBoxTest(udword nb0, const AABB** array0, udword nb1, const AABB** array1, Pairs& pairs);

#endif //__OPC_BOXPRUNING_H__
