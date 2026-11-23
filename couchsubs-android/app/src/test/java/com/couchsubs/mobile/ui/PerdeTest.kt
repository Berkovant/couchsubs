// SPDX-License-Identifier: AGPL-3.0-only
package com.couchsubs.mobile.ui

import org.junit.Assert.assertEquals
import org.junit.Assert.assertTrue
import org.junit.Test
import java.io.File

/** Perde (iPhone PerdeTests ile aynı kurallar): belirteçler tasarim.json'dan, hata metni ayrıştırma, kapak rengi, hareket kapısı. */
class PerdeTest {
    @Test
    fun tokensComeFromTasarimJson() {
        // DesignTokens derlemede tasarim.json'dan üretilir; koyu varsayılan ve Perde v2 değerleri.
        val json = File(System.getProperty("user.dir")).let { dir ->
            generateSequence(dir) { it.parentFile }.map { File(it, "design/yeni-arayuz/tasarim.json") }.first { it.exists() }
        }.readText()
        assertTrue(json.contains("\"version\": 2"))
        assertEquals(2, DesignTokens.VERSION)
        assertTrue(DesignTokens.DEFAULT_DARK)
        assertEquals(0xFF0F1013L, DesignTokens.Dark.background)
        assertEquals(0xFFF3F0EAL, DesignTokens.Light.background)
        // #RRGGBBAA → 0xAARRGGBB
        assertEquals(0x24F5B83DL, DesignTokens.Dark.amberSoft)
        assertEquals(16f, DesignTokens.Space.screenEdgeIOS)
        assertEquals(1600, DesignTokens.Motion.livePulse)
    }

    @Test
    fun errorTextSplitsIntoTitleStepsAndDetails() {
        val (title, steps, extra) = InstallErrorText.parse("TV yanıt vermedi. TV'yi açın. Aynı Wi-Fi'a bağlayın.\n\nAyrıntı: 192.168.1.5 sürüm 0.9.81")
        assertEquals("TV yanıt vermedi.", title)
        assertEquals(listOf("TV'yi açın.", "Aynı Wi-Fi'a bağlayın."), steps)
        assertEquals(listOf("Ayrıntı: 192.168.1.5 sürüm 0.9.81"), extra)
    }

    @Test
    fun coverPaletteMatchesIphoneHash() {
        // iPhone DSCoverArt.palette: 7 tohum, ×31 + karakter, 32 bit taşma, 6 palet.
        assertEquals(1, coverPaletteIndex("Death Stranding 2"))
        assertEquals("GTAV", monogram("Grand Theft Auto V"))
        assertEquals("RDR2", monogram("Red Dead Redemption 2"))
        assertEquals("MAF", monogram("Mafia"))
    }

    @Test
    fun motionGateStopsAmbientWhileTranslating() {
        assertTrue(MotionGate(reduced = false, translating = false, active = true).ambient)
        assertEquals(false, MotionGate(reduced = false, translating = true, active = true).ambient)
        assertTrue(MotionGate(reduced = false, translating = true, active = true).live)
        assertEquals(false, MotionGate(reduced = true, translating = false, active = true).live)
        assertEquals(false, MotionGate(reduced = false, translating = false, active = false).ambient)
    }
}
