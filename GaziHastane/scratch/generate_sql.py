import re

raw_data = """
Prof Dr. Abdulkadir Bedirli Genel Cerrahi Anabilimdalı
Prof. Dr. Abdullah Özer Kalp Ve Damar Cerrahisi Anabilimdalı
Prof. Dr. Abdullah Münci Yağcı İç Hastalıkları Hematoloji Bilim Dalı
Prof. Dr. Abdurrahman Tufan İç Hastalıkları Romatoloji Bilim Dalı
Prof. Dr. Adnan Abacı Kardiyoloji Anabilim Dalı
Prof. Dr. Ahmet Demircan Acil Tıp Anabilim Dalı
Prof. Dr. Ahmet Karamercan Genel Cerrahi Anabilim Dalı
Prof. Dr. Ahmet Özet İç Hastalıkları Onkoloji Bilim Dalı
Prof. Dr. Ahmet Erdem Kadın Hastalıkları ve Doğum Anabilim Dalı
Prof. Dr. Ahmet Baran Önal Radyoloji Anabilim Dalı
Prof. Dr. Ahmet Bora Küpeli Üroloji Anabilim Dalı
Prof. Dr. Ahmet Burhan Aksakal Deri Ve Zührevi Hastalıkları Anabilimdalı
Prof. Dr. Ahmet Memduh Kaymaz Beyin Ve Sinir Cerrahisi Anabilim Dalı
Prof. Dr. Ahmet Murad Hondur Göz Hastalıkları Anabilim Dalı
Prof. Dr. Ahmet Selim Yurdakul Göğüs Hastalıkları Anabilim Dalı
Prof. Dr. Ahmet Ziya Anadol Genel Cerrahi Anabilim Dalı
Prof. Dr. Akif Muhtar Öztürk Ortopedi Ve Travma. Anabilim Dalı
Prof. Dr. Alev Eroğlu Altınova İç Hastalıkları Endokrinoloji Bilim Dalı
Prof. Dr. Ali Çelik Göğüs Cerrahisi Anabilim Dalı
Prof. Dr. Ali Atan Üroloji Anabilim Dalı
Prof. Dr. Ali Ünsal Üroloji Anabilim Dalı
Prof. Dr. Ali Turgay Çavuşoğlu Ortopedi Ve Travma. Anabilim Dalı
Prof. Dr. Alp Özgün Börcek Beyin Ve Sinir Cerrahisi Anabilim Dalı
Prof. Dr. Alpaslan Şenköylü Ortopedi Ve Travma. Anabilim Dalı
Prof. Dr. Alper Ceylan Kulak Burun Boğaz Anabilim Dalı
Prof. Dr. Anıl Tapısız Çocuk Enfeksiyon Hastalıkları Bilimdalı
Prof. Dr. Arzu Okur Çocuk Hematoloji Bilim Dalı
Prof. Dr. Asife Şahinarslan Kardiyoloji Anabilim Dalı
Prof. Dr. Asiye Uğraş Dikmen Halk Sağlığı Anabilim Dalı
Prof. Dr. Aslı Kuruoğlu Ruh Sağlığı Ve Hastalıkları Anabilimdalı
Prof. Dr. Atiye Seda Yar Tıbbi Biyoloji Anabilim Dalı
Prof. Dr. Aydemir Kale Beyin Ve Sinir Cerrahisi Anabilim Dalı
Prof. Dr. Aydın Dalgıç Genel Cerrahi Anabilim Dalı
Prof. Dr. Ayfer Keleş Acil Tıp Anabilim Dalı
Prof. Dr. Aylar Poyraz Tıbbi Patoloji Anabilim Dalı
Prof. Dr. Aylin Sepici Dinçel Tıbbi Biyokimya Anabilim Dalı
Prof. Dr. Ayşe İriz Kulak Burun Boğaz Anabilim Dalı
Prof. Dr. Ayşe Gülşen Plastik, Rekonstrüktif Ve Estetik Cerrahi Anabilimdalı
Prof. Dr. Ayşe Kalkancı Tıbbi Mikrobiyoloji Anabilim Dalı
Prof. Dr. Ayşe Dursun Tıbbi Patoloji Anabilim Dalı
Prof. Dr. Ayşe Meltem Yalınay Tıbbi Mikrobiyoloji Anabilim Dalı
Prof. Dr. Ayşe Tana Aslan Çocuk Göğüs Hastalıkları Bilimdalı
Prof. Dr. Ayşegül Atak Yücel İmmünoloji Anabilim Dalı
Prof. Dr. Aysu Duyan Çamurdan Çocuk Sağlığı Ve Hastalıkları Anabilimdalı
Prof. Dr. Aysun Bideci Çocuk Endokrinoloji Bilimdalı
Prof. Dr. Aytuğ Üner İç Hastalıkları Onkoloji Bilim Dalı
Prof. Dr. Azime Şebnem Soysal Acar Çocuk Sağlığı Ve Hastalıkları Anabilimdalı
Doç. Dr. Abdulsamet Erden İç Hastalıkları Romatoloji Bilim Dal
Doç. Dr. Ahmet Çağrı Büyükkasap Genel Cerrahi Anabilim Dalı
Doç. Dr. Ahmet Özaslan Çocuk Ve Ergen Ruh Sağlığı Anabilim Dalıı
Doç. Dr. Ahmet Şeyhanlı İç Hastalıkları Hematoloji Bilim Dalı
Dr. Öğr. Üyesi Ahsen Öncü Erçelik Plastik, Rekonstrüktif Ve Estetik Cerrahi Anabilimdalı
Doç. Dr. Alim Can Baymurat Ortopedi Ve Travma. Anabilim Dalı
Doç. Dr. Aslı Akyol Gürses Nöroloji Anabilim Dalı
Doç. Dr. Aslı İnci Çocuk Metabolizma Ve Nutrisyon Bilimdalı
Doç. Dr. Aybeniz Civan Kahve Ruh Sağlığı Ve Hastalıkları Anabilimdalı
Doç. Dr. Ayça Utkan Karasu Fiziksel Tıp ve Rehabilitasyon Anabilim Dalı
Doç. Dr. Aydan Avdan Aslan Radyoloji Anabilim Dalı
Doç. Dr. Aydın Yavuz Genel Cerrahi Anabilim Dalı
Doç. Dr. Ayla Akca Çağlar Çocuk Sağlığı Ve Hastalıkları Anabilimdalı
Doç. Dr. Aylin Kılınç Uğurlu Çocuk Sağlığı Ve Hastalıkları Anabilimdalı
Doç. Dr. Ayhan Işık Erdal Plastik, Rekonstrüktif Ve Estetik Cerrahi Anabilimdalı
Dr. Öğr. Üyesi Ahmet Özaslan Çocuk Ruh Sağlığı ve Hastalıkları
Dr. Öğr. Üyesi Ahmet Yücel Üçgül Göz Hastalıkları Anabilim Dalı
Dr. Öğr. Üyesi Ali Eren Ortopedi Ve Travmatoloji Anabilim Dalı
Dr. Öğr. Üyesi Ali Karataş İç Hastalıkları Gastroenteroloji Bilim Dalı
Dr. Öğr. Üyesi Alper Özkök Adli Tıp Anabilim Dalı
Dr. Öğr. Üyesi Arın Tomruk Biyofizik Anabilim Dalı
Dr. Öğr. Üyesi Atiye Cenay Karabörk Kılıç Radyoloji Anabilim Dalı
Dr. Öğr. Üyesi Avniye Kübra Baskın Çocuk Sağlığı Ve Hastalıkları Anabilimdalı
Dr. Öğr. Üye. Ayla AKCA ÇAĞLAR Çocuk Acil Bilimdalı
Dr. Öğr. Üyesi Aynur Çoban Anatomi Bilimdalı
Dr. Öğr. Üyesi Ayşe Meltem Sevgili Fizyoloji Anabilim Dalı
Dr. Öğr. Üyesi Ayşe Soylu Anatomi Bilimdalı
Öğr. Gör. Dr. Alparslan Kapısız Çocuk Cerrahisi Anabilim Dalı
Öğr. Gör. Dr. Ayşe Şahin Adli Tıp Ana Bilim Dalı
Öğr. Gör. Dr. Akif Kavgacı Çocuk Sağlığı ve Hastalıkları Anabilim Dalı
Öğr. Gör. Dr. Aybala Nur Üçgül Radyasyon Onkolojisi Anabilim Dalı
Prof. Dr. Işıl Fidan Tıbbi Mikrobiyoloji Anabilim Dalı
Prof. Dr. İbrahim Murat Hırfanoğlu Yenidoğan (Neonatal) Bilimdalı
Prof. Dr. İhsan Gökhan Gürelik Göz Hastalıkları Anabilim Dalı
Prof. Dr. İlker Şen Üroloji Anabilim Dalı
Prof. Dr. İlyas Okur Çocuk Metabolizma Ve Nutrisyon Bilimdalı
Prof. Dr. İpek Işık Gönül Tıbbi Patoloji Anabilim Dalı
Prof. Dr. İpek Kıvılcım Oğuzülgen Göğüs Hastalıkları Anabilim Dalı
Prof. Dr. İrem Yıldırım Nöroloji Anabilim Dalı
Prof. Dr. İrfan Güngör Anestezi Ve Reanimasyon Anabilim Dalı
Prof. Dr. İsa Kılıçaslan Acil Tıp Anabilim Dalı
Prof. Dr. İsmail Cüneyt Kurul Göğüs Cerrahisi Anabilim Dalı
Prof. Dr. İsmail Güler Kadın Hastalıkları ve Doğum Anabilim Dalı
Prof. Dr. İsmet Bayramoğlu Kulak Burun Boğaz Anabilim Dalı
Doç. Dr. İsmail Akdulum Radyoloji Anabilim Dalı
Doç. Dr. İrem Ekmekci Ertek Ruh Sağlığı Ve Hastalıkları Anabilimdalı
Prof. Dr. Leyla Tümer Çocuk Metabolizma Ve Nutrisyon Bilimdalı
Prof. Dr. Lütfiye Özlem Atay Nükleer Tıp Anabilim Dalı
Doç. Dr. Levent Karataş Fiziksel Tıp ve Rehabilitasyon Anabilim Dalı
Prof. Dr. Oğuz Köktürk Göğüs Hastalıkları Anabilim Dalı
Prof. Dr. Okşan Derinöz Güleryüz Çocuk Acil Bilimdalı
Dr. Öğr. Üyesi Onur Aras Anatomi Bilimdalı
Prof. Dr. Onur Konuk Göz Hastalıkları Anabilim Dalı
Prof. Dr. Osman Kurukahvecioğlu Genel Cerrahi Anabilim Dalı
Prof. Dr. Osman Latifoğlu Plastik, Rekonstrüktif Ve Estetik Cerrahi Anabilim Dalı
Prof. Dr. Osman Yüksel Genel Cerrahi Anabilim Dalı
Prof. Dr. Ozan Yazıcı İç Hastalıkları Onkoloji Bilim Dalı
Doç. Dr. Onur İnam Biyofizik Anabilim Dalı
Doç. Dr. Osman Sütcüoğlu İç Hastalıkları Onkoloji Bilim Dalı
Ögr. Gör. Dr. Okan Ermiş Anestezi Ve Reanimasyon Anabilim Dalı
Ögr. Gör. Dr. Onur Ertunç Tıbbi Patoloji Anabilim Dalı
Öğr. Gör. Dr. Osman Tamer Şahin İç Hastalıkları Anabilim Dalı
Öğr. Gör. Dr. Oğuzhan Karasu Plastik, Rekonstrüktif Ve Estetik Cerrahi Anabilim Dalı
Prof. Dr. Ödül Eğritaş Gürkan Çocuk Gastroenteroloji Bilimdalı
Prof Dr. Ömer KURTİPEK Anestezi Ve Reanimasyon Anabilim Dalı
Prof. Dr. Ökkeş Sezai Leventoğlu Genel Cerrahi Anabilim Dalı
Prof. Dr. Özdemir Serhat Gürocak Üroloji Anabilim Dalı
Prof. Dr. Özge Petek Erpolat Radyasyon Onkolojisi Anabilim Dalı
Prof. Dr. Özlem Coşkun Nöroloji Anabilim Dalı
Prof. Dr. Özlem Coşkun Tıp Eğitimi Ve Bilişimi Anabilim Dalı
Prof. Dr. Özlem Erdem Tıbbi Patoloji Anabilim Dalı
Prof. Dr. Özlem Gülbahar Tıbbi Biyokimya Anabilim Dalı, Tıbbi Biyokimya Laboratuvarı
Prof. Dr. Özlem Güzel Tunçcan Enfeksiyon Hastalıkları Anabilim Dalı
Prof. Dr. Öznur Leman Boyunaga Radyoloji Anabilim Dalı
Doç. Dr. Özant Helvacı İç Hastalıkları Nefroloji Bilim Dalı
Doç. Dr. Özge Vural Çocuk Sağlığı Ve Hastalıkları Anabilimdalı
Doç. Dr. Özgür Ekinci Tıbbi Patoloji Anabilim Dalı
Doç Dr. Özgür Öcal Beyin ve Sinir Cerrahisi Anabilim Dalı
Doç. Dr. Ömer Faruk AKÇAY İç Hastalıkları Nefroloji Bilim Dalı
Dr. Öğr. Üyesi Özden Seçkin Kardiyoloji Anabilim Dalı
Dr. Öğr. Üyesi Özge Özgen Top Enfeksiyon Hastalıkları Anabilim Dalı
Dr. Öğr. Üyesi Özlem Dağlı Beyin Ve Sinir Cerrahisi Anabilim Dalı
Öğr. Gör. Dr. Özge Tonbuloğlu Altıner Halk Sağlığı Anabilim Dalı
Prof. Dr. Seçil Özkan Halk Sağlığı Anabilim Dalı
Prof. Dr. Sedat Türkoğlu Kardiyoloji Anabilim Dalı
Prof. Dr. Sefer Aycan Halk Sağlığı Anabilim Dalı
Prof. Dr. Selçuk Aslan Ruh Sağlığı Ve Hastalıkları Anabilimdalı
Prof. Dr. Selçuk Candansayar Ruh Sağlığı Ve Hastalıkları Anabilimdalı
Prof. Dr. Semiha Tokgöz Çocuk Kardiyoloji Bilimdalı
Prof. Dr. Serdar Kula Çocuk Kardiyoloji Bilimdalı
Prof. Dr. Serhat Avcu Radyoloji Anabilim Dalı
Prof. Dr. Sevcan Azime Bakkaloğlu Ezgü Çocuk Nefroloji Bilimdalı
Prof. Dr. Sevil Özger İlhan Tıbbi Farmakoloji Anabilim Dalı
Prof. Dr. Sevim Gönen Çocuk Nefroloji Bilimdalı
Prof. Dr. Sibel Dincer Fizyoloji Anabilim Dalı
Prof. Dr. Sinan Sarı Çocuk Gastroenteroloji Bilimdalı
Prof. Dr. Suna Ömeroğlu Histoloji Embriyoloji Anabilim Dalı
Prof. Dr. Süleyman Sabri Uslu Kulak Burun Boğaz Anabilim Dalı
Prof. Dr. Süleyman Yeşil Üroloji Anabilim Dalı
Prof. Dr. Süreyya Barun Tıbbi Farmakoloji Anabilim Dalı
Doç. Dr. Salih Topal Kardiyoloji Anabilim Dalı
Doç. Dr. Saygın Altıner Genel Cerrahi Anabilim Dalı
Doç. Dr. Selin Erel Anestezi Ve Reanimasyon Anabilim Dalı
Doç. Dr. Serap Çatlı Dinç Radyasyon Onkolojisi Anabilim Dalı
Doç.Dr. Serhat Çetin Üroloji Anabilim Dalı
Doç. Dr. Serhat Şibar Plastik, Rekonstrüktif Ve Estetik Cerrahi Anabilim Dalı
Doç. Dr. Serkan Ünlü Kardiyoloji Anabilim Dalı
Doç Dr. Sultan Pınar Çetintepe Halk Sağlığı Anabilim Dalı
Doç. Dr. Süleyman Cebeci Kulak Burun Boğaz Anabilim Dalı
Dr. Öğr. Üyesi Secdegül Coşkun Yaş Acil Tıp Anabilim Dalı
Dr. Öğr. Üyesi Seda Gülbahar Ateş Nükleer Tıp Anabilim Dalı
Dr. Öğr. Üyesi Serap Kirkiz Kayalı Çocuk Hematoloji Bilimdalı
Dr. Öğr. Üyesi Süheyla Esra Özkoçer Histoloji Embriyoloji Anabilim Dalı
Dr. Öğr. Üyesi Sercan Tak Kalp Ve Damar Cerrahisi Anabilim Dalı
Dr. Öğr. Üyesi Sevcihan Kesen Özbek Radyoloji Anabilim Dalı
Ögr. Gör. Dr. Sidre Erganiş Tıbbi Mikrobiyoloji Anabilim Dalı
Öğr. Gör. Dr. Sümeyye Kodalak Cengiz Göğüs Hastalıkları Anablim Dalı
Öğr. Gör. Dr. Selim Keçeoğlu Genel Cerrahi Anabilim Dalı
Prof. Dr. Ülker Koçak Çocuk Hematoloji Bilimdalı
Prof. Dr. Ülkü Nesrin Demirsoy Fiziksel Tıp ve Rehabilitasyon Anabilim Dalı
Prof. Dr. Ülver Derici İç Hastalıkları Nefroloji Bilim Dalı
Prof. Dr. Ümit Özgür Akdemir Nükleer Tıp Anabilim Dalı
Doç. Dr. Ümmügülsüm Gaygısız Anestezi Ve Reanimasyon Anabilim Dalı
Prof. Dr. Bahar Çuhacı Çakır Çocuk Sağlığı Ve Hastalıkları Anabilimdalı
Prof. Dr. Bahri Aydın Göz Hastalıkları Anabilim Dalı
Prof. Dr. Bahriye Aral Biyofizik Anabilim Dalı
Prof. Dr. Bayazit Dikmen Anestezi Ve Reanimasyon Anabilim Dal
Prof. Dr. Behçet Coşar Ruh Sağlığı Ve Hastalıkları Anabilimdalı
Prof. Dr. Berna Göker İç Hastalıkları Romatoloji Bilim Dalı, Geriatri Bilim Dalı
Prof. Dr. Berrin Işık Anestezi Ve Reanimasyon Anabilim Dalı
Prof. Dr. Bijen Nazlıel Nöroloji Anabilim Dalı
Prof. Dr. Birol Demirel Adli Tıp Anabilim Dalı
Prof. Dr. Buket Dalgıç Çocuk Gastroenteroloji Bilimdalı
Prof. Dr. Bülent Cengiz Nöroloji Anabilim Dalı
Doç. Dr. Bahar Öztelcan Gündüz Çocuk Sağlığı Ve Hastalıkları Anabilimdalı
Doç. Dr. Bekir Eren Çetin Radyasyon Onkolojisi Anabilim Dalı
Doç. Dr. Betül Seher Uysal Göz Hastalıkları Anabilim Dalı
Doç. Dr. Betül Öğüt Cimer Tıbbi Patoloji Anabilim Dalı
Doç. Dr. Buket Koparal Ruh Sağlığı Ve Hastalıkları Anabilimdalı
Doç. Dr. Burak Karaaslan Beyin Ve Sinir Cerrahisi Anabilim Dalı
Doç. Dr. Burak Sezenöz Kardiyoloji Anabilim Dalı
Dr. Öğr. Üyesi Banu Tijen Ceylan Kulak Burun Boğaz Anabilim Dalı
Dr. Öğr. Üyesi Berçin Tarlan Göz Hastalıkları Anabilim Dalı
Dr. Öğr. Üyesi Berkay Temel Deri ve Zührevi Hastalıları Anabilim Dalı
Dr. Öğr. Üyesi Burak Kayabaşı Fizyoloji Anabilim Dalı
Dr. Öğr. Üyesi Burcu Beksaç Deri Ve Zührevi Hastalıkları Anabilimdalı
Dr. Öğr. Üyesi Burcu Küçük Biçer Tıp Eğitimi Ve Bilişimi Anabilim Dalı
Dr. Öğr. Üyesi Burcu Yazıcıoğlu Çocuk Nefroloji Bilimdalı
Ögr. Gör. Dr. Başak Koçak Kalp ve Damar Cerrahisi Anabilim Dalı
Prof. Dr. Ebru Arhan Çocuk Nöroloji Bilimdalı
Ögr. Gör. Dr. Ebru Şansal Kulak Burun Boğaz Anabilim Dalı
Prof. Dr. Ece Konaç Tıbbi Biyoloji Anabilim Dalı
Prof. Dr. Ekmel Tezel Genel Cerrahi Anabilim Dalı
Ögr. Gör. Dr. Elçin Orçan Kulak Burun Boğaz Anabilim Dalı
Prof. Dr. Elçin Özgür Büyükatalay Biyofizik Anabilim Dalı
Prof. Dr. Elvan İşeri Çocuk Ve Ergen Ruh Sağlığı Anabilim Dalı
Prof. Dr. Emin Ümit Bağrıaçık İmmünoloji Anabilim Dalı
Prof. Dr. Emine Şamdancı Tıbbi Patoloji Anabilim Dalı
Prof. Dr. Emine Belgin Koçer Nöroloji Anabilim Dalı
Prof. Dr. Eray Karahacıoğlu Radyasyon Onkolojisi Anabilim Dalı
Prof. Dr. Eray Esra Önal Yenidoğan (Neonatal) Bilimdalı
Prof. Dr. Ercan Demir Çocuk Nöroloji Bilimdalı
Prof. Dr. Erdem Yüksel Göz Hastalıkları Anabilim Dalı
Prof. Dr. Erdinç Esen Ortopedi Ve Travma. Anabilim Dalı
Prof. Dr. Erhan Turgut Ilgıt Radyoloji Anabilim Dalı
Prof. Dr. Erkan İriz Kalp Ve Damar Cerrahisi Anabilimdalı
Prof. Dr. Esin Koç Yenidoğan (Neonatal) Bilimdalı
Prof. Dr. Esin Şenol Enfeksiyon Hastalıkları Anabilim Dalı
Prof. Dr. Esra Adışen Deri Ve Zührevi Hastalıkları Anabilimdalı
Prof. Dr. Esra Güney Çocuk Ve Ergen Ruh Sağlığı Anabilim Dalı
Ögr. Gör. Dr. Esra Karabay Kadın Hastalıkları ve Doğum Anabilim Dalı
Prof. Dr. Esra Tuğ Tıbbi Genetik Anabilim Dalı
Prof. Dr. Ethem Turgay Cerit İç Hastalıkları Endokrinoloji Bilim Dalı
Doç. Dr. Ebru Azapağası Çocuk Sağlığı Ve Hastalıkları Anabilimdalı
Doç. Dr. Emel Güler Algoloji Bilim Dalı
Doç. Dr. Emetullah Cindil Radyoloji Anabilim Dalı
Doç. Dr. Emrah Çeltikçi Beyin Ve Sinir Cerrahisi Anabilim Dalı
Doç. Dr. Emrullah Kızıltunç Kardiyoloji Anabilim Dalı
Doç. Dr. Ender Cem Bulut Üroloji Anabilim Dalı
Doç. Dr. Erdem Aras Sezgin Ortopedi Ve Travma. Anabilim Dalı
Doç. Dr. Ergin Dileköz Tıbbi Farmakoloji Anabilim Dalı
Doç. Dr. Erhan Demirdağ Kadın Hastalıkları ve Doğum Anabilim Dalı
Doç. Dr. Esra İşçi Bostancı Kadın Hastalıkları ve Doğum Anabilim Dalı
Doç. Dr. Esra Serdaroğlu Çocuk Nöroloji Bilimdalı
Dr. Öğr. Üyesi Elif Ayça Şahin Tıbbi Mikrobiyoloji Anabilim Dalı
Dr. Öğr. Üyesi Erdem Balcı Nükleer Tıp Anabilim Dalı
Dr. Öğr. Üyesi Ertuğrul Çağrı Bölek Romatoloji Anabilim Dalı
Dr. Öğr. Üyesi Emre Leventoğlu Çocuk Nefroloji Bilim Dalı
Öğr. Gör. Dr. Elif Kolay Bayram Patoloji Anabilim Dalı
Ögr. Gör. Dr. Ercan Yıldırım Anestezi Ve Reanimasyon Anabilim Dalı
Ögr. Gör. Dr. Esra Döğer Çocuk Endokrinoloji Bilimdalı
Öğr. Gör. Dr. Ekin Kartal Anatomi Bilimdalı
Öğr. Gör. Dr. Elif Gülçiçek Abbasoğlu Topa Nöroloji Anabilim Dalı
Öğr. Gör. Dr. Esra Gültekin Koç Halk Sağlığı Anabilim Dalı
Öğr. Gör. Dr. Emine Merve Savaş İç Hastalıkları Anabilim Dalı
Prof. Dr. Hacer İlbilge Ertoy Karagöl Çocuk Alerji Ve Astım Bilimdalı
Prof. Dr. Hacı Hulusi Kafalıgönül Anatomi Anabilim Dalı
Prof. Dr. Hakan Atalar Ortopedi Ve Travma. Anabilim Dalı
Prof. Dr. Hakan Tutar Kulak Burun Boğaz Anabilim Dalı
Prof. Dr. Hakan Yusuf Selek Ortopedi Ve Travma. Anabilim Dalı
Prof. Dr. Hale Zeynep Batur Çağlayan Nöroloji Anabilim Dalı
Prof. Dr. Hamit Küçük İç Hastalıkları Romatoloji Bilim Dalı
Prof. Dr. Hamza Özer Ortopedi Ve Travma. Anabilim Dalı
Prof. Dr. Hasan Bostancı Genel Cerrahi Anabilim Dalı
Prof. Dr. Hasan Tezer Çocuk Enfeksiyon Hastalıkları Bilimdalı
Prof. Dr. Hasan Kutluk Pampal Anestezi Ve Reanimasyon Anabilim Dalı
Prof. Dr. Hatice Paşaoğlu Tıbbi Biyokimya Anabilim Dalı
Prof. Dr. Hatice Ayşe Bora Nöroloji Anabilim Dalı
Prof. Dr. Hatice Kibriya Fidan Çocuk Nefroloji Bilimdalı
Prof. Dr. Hatice Tuba Atalay Göz Hastalıkları Anabilim Dalı
Prof. Dr. Hayrunnisa Bolay Belen Nöroloji Anabilim Dalı
Prof. Dr. Hidayet Reha Kuruoğlu Nöroloji Anabilim Dalı
Prof. Dr. Hikmet Selçuk Gedik Kalp Ve Damar Cerrahisi Anabilimdalı
Prof. Dr. Hüseyin Bora Radyasyon Onkolojisi Anabilim Dalı
Prof. Dr. Hüseyin Koray Kılıç Radyoloji Anabilim Dalı
Prof. Dr. Hüseyin Murat Özdemir Kardiyoloji Anabilim Dalı
Doç. Dr. Evren Boran Nöroloji Anabilim Dalı
Doç. Dr. Hacer Doğan Varan Geriatri Bilim Dalı
Doç. Dr. Hacer İlke Önen Tıbbi Biyoloji Anabilim Dalı
Doç. Dr. Hakan Tüzün Halk Sağlığı Anabilim Dalı
Doç. Dr. Halit Nahit Şendur Radyoloji Anabilim Dalı
Doç. Dr. Hüseyin Baran Özdemir Göz Hastalıkları Anabilim Dalı
Doç. Dr. Hüseyin Demirtaş Kalp Ve Damar Cerrahisi Anabilimdalı
Doç. Dr. Hüseyin Göbüt Genel Cerrahi Anabilim Dalı
Dr. Öğr. Üyesi Hakan Öztürk Çocuk Gastroenteroloji Bilimdalı
Ögr. Gör. Dr. Hilal Korkmaz Fizyoloji Anabilim Dalı
Ögr. Gör. Dr. Hilal Saraç Canbolat Radyasyon Onkolojisi Anabilim Dalı
Öğr. Gör. Dr. Hatice Betül Moğulkoç Fizyoloji Anabilim Dalı
Öğr. Gör. Dr. Hanife Saat Tıbbi Genetik Anabilim Dalı
Prof. Dr. Nalan Akyürek Tıbbi Patoloji Anabilim Dalı
Doç. Dr. Namık Çencen Tıp Tarihi Ve Etik Anabilim Dalı
Prof. Dr. Neslihan Bukan Tıbbi Biyokimya Anabilim Dalı
Prof. Dr. Nesrin Çobanoğlu Tıp Tarihi Ve Etik Anabilim Dalı
Prof. Dr. Neşe Karabacak Nükleer Tıp Anabilim Dalı
Prof. Dr. Nigar Esra Erkoç Ataoğlu Nöroloji Anabilim Dalı
Prof. Dr. Nil Tokgöz Radyoloji Anabilim Dalı
Prof. Dr. Nilgün Yılmaz Demirci Göğüs Hastalıkları Anabilim Dalı
Prof. Dr. Nilsel İlter Deri Ve Zührevi Hastalıkları Anabilimdalı
Prof. Dr. Nuray Bozkurt Kadın Hastalıkları ve Doğum Anabilim Dalı
Prof. Dr. Nurdan Bedirli Anestezi Ve Reanimasyon Anabilim Dalı
Prof. Dr. Nuriye Ebru Ergenekon Yenidoğan (Neonatal) Bilimdalı
Prof. Dr. Nuriye Özdemir İç Hastalıkları Onkoloji Bilim Dalı
Prof. Dr. Nurten İnan Algoloji Bilim Dalı
Doç. Dr. Nazmi Mutlu Karakaş Çocuk Sağlığı Ve Hastalıkları Anabilimdalı
Doç. Dr. Nevin Taci Hoca Göğüs Hastalıkları Anabilim Dalı
Doç. Dr. Nuray Camgöz Eryılmaz Anestezi Ve Reanimasyon Anabilim Dalı
Doç. Dr. Nuray Varol Tıbbi Biyoloji Anabilim Dalı
Doç. Dr. Nazlıhan Boyacı Dündar İç Hastalıkları Yoğun Bakım
Dr. Öğr. Üyesi Niyazi Samet Yılmaz Tıbbi Biyokimya Anabilim Dalı
Ögr. Gör. Dr. Nihan Örüklü İmmünoloji Anabilim Dalı
Prof. Dr. Pınar Tokdemir Çalış Kadın Hastalıkları ve Doğum Anabilim Dalı
Doç. Dr. Pınar Aysert Yıldız Enfeksiyon Hastalıkları Anabilim Dalı
Doç. Dr. Pelin Kuzucu Beyin Ve Sinir Cerrahisi Anabilim Dalı
Doç. Dr. Pelin Telkoparan Akıllılar Tıbbi Biyoloji Anabilim Dalı
Dr. Öğr. Üyesi Pelin Bayık Tıbbi Patoloji Anabilim Dalı
Ögr. Gör. Dr. Pelin Türkkan Fizyoloji Anabilim Dalı
Prof. Dr. Ramazan Karabulut Çocuk Cerrahisi Anabilim Dalı
Prof. Dr. Resul Karakuş İmmünoloji Anabilim Dalı
Prof. Dr. Rukiye Filiz Karadağ Ruh Sağlığı Ve Hastalıkları Anabilimdalı
Doç. Dr. Ramazan Kozan Genel Cerrahi Anabilim Dalı
Doç. Dr. Recep Karamert Kulak Burun Boğaz Anabilim Dalı
Doç. Dr. Rezan Koçak Ulucaköy Fiziksel Tıp ve Rehabilitasyon Anabilim Dalı
Dr. Öğr. Üyesi Rıdvan Murat ÖKTEM Çocuk Sağlığı Ve Hastalıkları Anabilimdalı
Prof. Dr. Şahender Gülbin Aygencel Bıkmaz İç Hastalıkları Yoğun Bakım
Prof. Dr. Şefik Murat Arıkan Ortopedi Ve Travma. Anabilim Dalı
Prof. Dr. Şengül Özdek Göz Hastalıkları Anabilim Dalı
Prof. Dr. Şevin Güney Fizyoloji Anabilim Dalı
Dr. Öğr. Üyesi Şevki Mustafa Demiröz Göğüs Cerrahisi Anabilim Dalı
Prof. Dr. Taner Akar Adli Tıp Anabilim Dalı
Prof. Dr. Tansu Ulukavak Çiftçi Göğüs Hastalıkları Anabilim Dalı
Prof. Dr. Tarkan Karakan İç Hastalıkları Gastroenteroloji Bilim Dalı
Ögr. Gör. Dr. Temur Demirci Radyasyon Onkolojisi Anabilim Dalı
Prof. Dr. Tevfik Sinan Sözen Üroloji Anabilim Dalı
Prof. Dr. Tuğba Hırfanoğlu Çocuk Nöroloji Bilimdalı
Prof. Dr. Tuğba Şişmanlar Eyüboğlu Çocuk Sağlığı Ve Hastalıkları Anabilim Dalı
Prof. Dr. Tuncay Nas Kadın Hastalıkları ve Doğum Anabilim Dalı
Prof. Dr. Tuncay Veysel Peker Anatomi Anabilim Dalı
Prof. Dr. Türkan Aydın Teke Çocuk Enfeksiyon Hastalıkları Bilim Dalı
Doç. Dr. Toygun Kağan Eren Ortopedi ve Travmatoloji Anabilim Dalı
Doç. Dr. Tuğçe Tural Kara Çocuk Enfeksiyon Hastalıkları Bilim Dalı
Doç. Dr. Tuğba Bedir Demirdağ Çocuk Enfeksiyon Hastalıkları Bilim Dalı
Doç. Dr. Tuğba Çelen Yoldaş Çocuk Sağlığı Ve Hastalıkları Anabilim Dalı
Dr. Öğr. Üyesi Taylan Altıparmak Nöroloji Anabilim Dalı
Prof. Dr. Vedat Bulut İmmünoloji Anabilim Dalı
Prof. Dr. Volkan Sinci Kalp Ve Damar Cerrahisi Anabilimdalı
Doç. Dr. Volkan Medeni Halk Sağlığı Anabilim Dalı
Doç. Dr. Volkan Şıvgın Anestezi Ve Reanimasyon Anabilim Dalı
Doç. Dr. Vildan Özeke Tıp Eğitimi Ve Bilişimi Anabilim Dalı
Prof. Dr. Zafer Günendi Fiziksel Tıp ve Rehabilitasyon Anabilim Dalı, Fiziksel Tıp ve Rehabilitasyon Romatoloji Bilim Dalı
Prof. Dr. Zafer Kutay Coşkun Anatomi Anabilim Dalı
Prof. Dr. Zafer Türkyılmaz Çocuk Cerrahisi Anabilim Dalı
Prof. Dr. Zekeriya Ülger Geriatri Bilim Dalı
Ögr. Gör. Dr. Zehra Baltacı İç Hastalıkları Anabilim Dalı
Doç. Dr. Zeliha Aycan Özdemirkan Anestezi Ve Reanimasyon Anabilim Dalı
Prof. Dr. Zerrin Özköse Şatırlar Anestezi Ve Reanimasyon Anabilim Dalı
Prof. Dr. Zeynep Arzu Yegin İç Hastalıkları Hematoloji Bilim Dalı
Prof. Dr. Zübeyde Nur Özkurt İç Hastalıkları Hematoloji Bilim Dalı
Prof. Dr. Zühre Kaya Çocuk Hematoloji Bilimdalı
Doç. Dr. Zeliha Aycan Özdemirkan Anestezi Ve Reanimasyon Anabilim Dalı
Doç. Dr. Zeynep Öztürk Çocuk Nöroloji Bilim Dalı
Dr. Öğr. Üyesi Zeynep Yığman Histoloji Embriyoloji Anabilim Dalı
Prof. Dr. Canan Uluoğlu Tıbbi Farmakoloji Anabilim Dalı
Prof. Dr. Canan Türkyılmaz Yenidoğan (Neonatal) Bilimdalı
Prof. Dr. Cemalettin Aybay İmmünoloji Anabilim Dalı
Prof. Dr. Cemile Merve Seymen Histoloji Embriyoloji Anabilim Dalı
Prof. Dr. Cengiz Karakaya Tıbbi Biyokimya Anabilim Dalı
Doç Dr. Cem Kaya Çocuk Cerrahisi Anabilim Dalı
Dr. Öğr. Üyesi Cansu Özbaş Halk Sağlığı Anabilim Dalı
Ögr. Gör. Dr. Ceren Hançer Radyasyon Onkolojisi Anabilim Dalı
Prof. Dr. Çiğdem Özer Fizyoloji Anabilim Dalı
Prof. Dr. Çiğdem Elmas Histoloji Embriyoloji Anabilim Dalı
Prof. Dr. Çiğdem Özer Fizyoloji Anabilim Dalı
Prof. Dr. Çiğdem Seher Kasapkara Çocuk Sağlığı ve Hastalıkları Ana Bilim Dalı
Prof. Dr. Çağdaş Kalkan İç Hastalıkları Anabilim Dalı
Doç Dr. Çisem Yıldız Yıldırım Çocuk Sağlığı ve Hastalıkları Anabilim Dalı
Dr. Öğr. Üyesi Çağrı Özdemir Anestezi Ve Reanimasyon Anabilim Dalı
Prof. Dr. Demet Coşkun Anestezi Ve Reanimasyon Anabilim Dalı
Prof. Dr. Deniz Aslan Çocuk Hematoloji Bilimdalı
Prof. Dr. Deniz Karçaaltıncaba Kadın Hastalıkları ve Doğum Anabilim Dalı
Prof. Dr. Didem Tuba Akçalı Algoloji Bilim Dalı
Prof. Dr. Dudu Berrin Günaydın Anestezi Ve Reanimasyon Anabilim Dalı
Doç. Dr. Deniz Gezgin Yıldırım Çocuk Romatoloji Bilimdalı
Doç Dr. Doğa Vurallı Nöroloji Anabilim Dalı
Dr. Öğr. Üyesi Dicle Büyüktaşkın Tunçtürk Çocuk Ve Ergen Ruh Sağlığı Anabilim Dalı
Dr. Öğr. Üyesi Duygu Dayanır Histoloji Embriyoloji Anabilim Dalı
Dr. Öğr. Üyesi Duygu Deniz Usta Salimi Tıbbi Biyoloji Anabilim Dalı
Prof. Dr. Faruk Güçlü Pınarlı Çocuk Hematoloji Bilimdalı
Prof. Dr. Fatih Süheyl Ezgü Çocuk Metabolizma Ve Nutrisyon Bilimdalı
Prof. Dr. Fatma Nur Baran Aksakal Halk Sağlığı Anabilim Dalı
Prof. Dr. Fazlı Polat Üroloji Anabilim Dalı
Prof. Dr. Feride Nur Göğüş Fiziksel Tıp ve Rehabilitasyon Anabilim Dalı, Fiziksel Tıp ve Rehabilitasyon Romatoloji Bilim Dalı
Prof. Dr. Feriha Pınar Uyar Göçün Tıbbi Patoloji Anabilim Dalı
Prof. Dr. Fikret Bildik Acil Tıp Anabilim Dalı
Prof. Dr. Fikret Hüseyin Doğulu Beyin Ve Sinir Cerrahisi Anabilim Dalı
Prof. Dr. Funda Doğruman Al Tıbbi Mikrobiyoloji Anabilim Dalı
Prof. Dr. Füsun Saadet Törüner İç Hastalıkları Endokrinoloji Bilim Dalı
Doç. Dr. Fatih Gürler İç Hastalıkları Onkoloji Bilim Dalı
Doç. Dr. Funda Tamer Deri Ve Zührevi Hastalıkları Anabilimdalı
Dr. Öğr. Üyesi Fatih Öncü Radyoloji Anabilim Dalı
Dr. Öğr. Üyesi Fatma Hayvacı Canbeyli Çocuk Kardiyoloji Bilimdalı
Dr. Öğr. Üyesi Funda YILDIRIM BORAZAN İç Hastalıkları Anabilim Dalı, Geriatri Bilim Dalı
Ögr. Gör. Dr. Filiz Başak Ergin Çocuk Sağlığı ve Hastalıkları Anabilimdalı
Prof. Dr. Galip Güz İç Hastalıkları Nefroloji Bilim Dalı
Prof. Dr. Gökhan Kurt Beyin Ve Sinir Cerrahisi Anabilim Dalı
Prof. Dr. Gökhan Tuna Öztürk Fiziksel Tıp ve Rehabilitasyon Anabilim Dalı
Prof. Dr. Göknur Güler Öztürk Biyofizik Anabilim Dalı
Ögr. Gör. Dr. Gözde Bayramoğlu Çabuk Kulak Burun Boğaz Anabilim Dalı
Prof. Dr. Gonca Erbaş Radyoloji Anabilim Dalı
Prof. Dr. Gül Gürsel Göğüs Hastalıkları Yoğun Bakım
Prof. Dr. Gülçin Kaymak Karataş Fiziksel Tıp ve Rehabilitasyon Anabilim Dalı
Prof. Dr. Güldal Esendağlı Tıbbi Patoloji Anabilim Dalı
Prof. Dr. Gülen Akyol Tıbbi Patoloji Anabilim Dalı
Prof. Dr. Gülendam Bozdayı Tıbbi Mikrobiyoloji Anabilim Dalı
Prof. Dr. Gülnur Take Kaplanoğlu Histoloji Embriyoloji Anabilim Dalı
Prof. Dr. Gülten Taçoy Kardiyoloji Anabilim Dalı
Prof. Dr. Gürsel Levent Oktar Kalp Ve Damar Cerrahisi Anabilimdalı
Prof. Dr. Gökçe Sevim Fincan Öztürk Tıbbi Farmakoloji Anabilim Dalı
Prof. Dr. Gülay Kip Anestezi Ve Reanimasyon Anabilim Dalı
Doç. Dr. Gökçen Emmez Anestezi Ve Reanimasyon Anabilim Dalı
Doç. Dr. Gözde İnan Anestezi Ve Reanimasyon Anabilim Dalı
Doç. Dr. Güner Kılıç İç Hastalıkları Gastroenteroloji Bilim Dalı
Dr. Öğr. Üyesi Gözde Savaş İç Hastalıkları Onkoloji Bilim Dalı
Dr. Öğr. Üyesi Gültekin Kadı Acil Tıp Anabilim Dalı
Prof. Dr. Göktürk Fındık Göğüs Cerrahisi Anabilim Dalı
Prof. Dr. Kaan Sönmez Çocuk Cerrahisi Anabilim Dalı
Prof. Dr. Kadir Kemal Uygur Kulak Burun Boğaz Anabilim Dalı
Prof. Dr. Kadriye Altok İç Hastalıkları Nefroloji Bilim Dalı
Prof. Dr. Kayhan Çağlar Tıbbi Mikrobiyoloji Anabilim Dalı
Prof. Dr. Kazime Gonca Akbulut Fizyoloji Anabilim Dalı
Prof. Dr. Kemal Fındıkçıoğlu Plastik, Rekonstrüktif Ve Estetik Cerrahi Anabilimdalı
Prof. Dr. Kürşat Dikmen Genel Cerrahi Anabilim Dalı
Doç. Dr. Kamil İnci İç Hastalıkları Yoğun Bakım
Ögr. Gör. Dr. Kübra Gizem Esentürk Yayla Tıbbi Biyoloji Anabilim Dalı
Prof. Dr. Kübranur Ünal Tıbbi Biyokimya Anabilim Dalı
Dr. Öğr. Üyesi Kerem Atalar Anatomi Anabilim Dalı
Prof. Dr. Mahmut Orhun Çamurdan Çocuk Endokrinoloji Bilimdalı
Prof. Dr. Mehmet Akif Karamercan Acil Tıp Anabilim Dalı
Prof. Dr. Mehmet Akif Öztürk İç Hastalıkları Romatoloji Bilim Dalı
Prof. Dr. Mehmet Akif Türkoğlu Genel Cerrahi Anabilim Dalı
Prof. Dr. Mehmet Ali Aslaner Acil Tıp Anabilim Dalı
Prof. Dr. Mehmet Ali Ergün Tıbbi Genetik Anabilim Dalı
Prof. Dr. Mehmet Anıl Onan Kadın Hastalıkları ve Doğum Anabilim Dalı
Prof. Dr. Mehmet Araç Radyoloji Anabilim Dalı
Prof. Dr. Mehmet Ayhan Karakoç İç Hastalıkları Endokrinoloji Bilim Dalı
Prof. Dr. Mehmet Birol Uğur Kulak Burun Boğaz Anabilim Dalı
Prof. Dr. Mehmet Cindoruk İç Hastalıkları Gastroenteroloji Bilim Dalı
Prof. Dr. Mehmet Cüneyt Özmen Göz Hastalıkları Anabilim Dalı
Prof. Dr. Mehmet Düzlü Kulak Burun Boğaz Anabilim Dalı
Prof. Dr. Mehmet Erdem Kadın Hastalıkları ve Doğum Anabilim Dalı
Prof. Dr. Mehmet Kadri Akboğa Kardiyoloji Anabilim Dalı
Prof. Dr. Mehmet Koray Akkan Radyoloji Anabilim Dalı
Prof. Dr. Mehmet Muhittin Yalçın İç Hastalıkları Endokrinoloji Bilim Dalı
Prof. Dr. Mehmet Rıdvan Yalçın Kardiyoloji Anabilim Dalı
Prof. Dr. Mehmet Sühan Ayhan Plastik, Rekonstrüktif Ve Estetik Cerrahi Anabilim Dalı
Prof. Dr. Mehmet Zeki Taner Kadın Hastalıkları ve Doğum Anabilim Dalı
Prof. Dr. Melda Türkoğlu İç Hastalıkları Yoğun Bakım
Prof. Dr. Meltem Bahçelioğlu Anatomi Anabilim Dalı
Prof. Dr. Meltem Polat Çocuk Sağlığı Ve Hastalıkları Anabilim Dalı
Prof. Dr. Meral Yirmibeş Karaoğuz Tıbbi Genetik Anabilim Dalı
Prof. Dr. Meriç Arda Eşmekaya Biyofizik Anabilim Dalı
Prof. Dr. Mertihan Kurdoğlu Kadın Hastalıkları ve Doğum Anabilim Dalı
Prof. Dr. Mesut Emre Yaman Beyin Ve Sinir Cerrahisi Anabilim Dalı
Prof. Dr. Mesut Öktem Kadın Hastalıkları ve Doğum Anabilim Dalı
Prof. Dr. Metin Alkan Anestezi Ve Reanimasyon Anabilim Dalı
Prof. Dr. Metin Onaran Üroloji Anabilim Dalı
Prof. Dr. Metin Yılmaz Kulak Burun Boğaz Anabilim Dalı
Prof. Dr. Muhammet Baybars Ataoğlu Ortopedi Ve Travma. Anabilim Dalı
Prof. Dr. Muhterem Polat Deri Ve Zührevi Hastalıkları Anabilimdalı
Prof. Dr. Murat Akın Genel Cerrahi Anabilim Dalı
Prof. Dr. Murat Dizbay Enfeksiyon Hastalıkları Anabilim Dalı
Prof. Dr. Murat Kekilli İç Hastalıkları Gastroenteroloji Bilim Dalı
Prof. Dr. Murat Orhan Öztaş Deri Ve Zührevi Hastalıkları Anabilimdalı
Prof. Dr. Murat Uçar Radyoloji Anabilim Dalı
Prof. Dr. Murat Zinnuroğlu Fiziksel Tıp ve Rehabilitasyon Anabilim Dalı, iziksel Tıp ve Rehabilitasyon Algoloji Bilim Dalı
Prof. Dr. Mustafa Arslan Anestezi Ve Reanimasyon Anabilim Dalı
Prof. Dr. Mustafa Cemri Kardiyoloji Anabilim Dalı
Prof. Dr. Mustafa Hakan Sözen Genel Cerrahi Anabilim Dalı
Prof. Dr. Mustafa Hakan Zor Kalp Ve Damar Cerrahisi Anabilimdalı
Prof. Dr. Mustafa Kamil Bilgihan Göz Hastalıkları Anabilim Dalı
Prof. Dr. Mustafa Kavutçu Tıbbi Biyokimya Anabilim Dalı
Prof. Dr. Mustafa Kerem Genel Cerrahi Anabilim Dalı
Prof. Dr. Mustafa Necmi İlhan Halk Sağlığı Anabilim Dalı
Prof. Dr. Mustafa Özgür Tan Üroloji Anabilim Dalı
Prof. Dr. Mustafa Şare Genel Cerrahi Anabilim Dalı
Prof. Dr. Mutlu Uysal Yazıcı Çocuk Sağlığı Ve Hastalıkları Anabilim Dalı
Prof. Dr. Müge Aydoğdu Göğüs Hastalıkları Yoğun Bakım
Prof. Dr. Müjde Yaşım Aktürk İç Hastalıkları Endokrinoloji Bilim Dalı
Doç. Dr. Mahi Nur Cerit Radyoloji Anabilim Dalı
Doç. Dr. Mehmet Ali Tokgöz Ortopedi Ve Travma. Anabilim Dalı
Doç. Dr. Muammer Melih Şahin Kulak Burun Boğaz Anabilim Dalı
Doç. Dr. Muhammed Hakan Aksu Ruh Sağlığı Ve Hastalıkları Anabilim Dalı
Doç. Dr. Muhammet Sayan Göğüs Cerrahisi Anabilim Dalı
Doç. Dr. Murat Yavuz Koparal Üroloji Anabilim Dalı
Doç. Dr. Mustafa Candemir Kardiyoloji Anabilim Dalı
Doç. Dr. Münire Funda Cevher Akdulum Kadın Hastalıkları ve Doğum Anabilim Dalı
Dr. Öğr. Üyesi Mehmet Arda İnan Tıbbi Patoloji Anabilim Dalı
Dr. Öğr. Üyesi Mehmet Eren Zorlu Kulak Burun Boğaz Hastalıkları Anabilim Dalı
Dr. Öğr. Üyesi Melek Yaman İmmünoloji Anabilim Dalı
Dr. Öğr. Üyesi Merve Bahar Ercan Nöroloji Anabilim Dalı
Dr. Öğr. Üyesi Merve Yazol Radyoloji Anabilim Dalı
Dr. Öğr. Üyesi Mesut Yavaş Genel Cerrahi Anabilim Dalı
Dr. Öğr. Üyesi Muhammed Ertuğrul ŞENTÜRK Radyasyon Onkolojisi
Dr. Öğr. Gör. Muhammed Civan Güvel Tıbbi Farmakoloji Anabilim Dalı
Öğr. Gör. Dr. Mert Polat Kadın Hastalıkları ve Doğum Anabilim Dalı
Prof. Dr. Uğur Coşkun İç Hastalıkları Onkoloji Bilim Dalı
Prof. Dr. Ulunay Kanatlı Ortopedi Ve Travma. Anabilim Dalı
Prof. Dr. Utku Aydil Kulak Burun Boğaz Anabilim Dalı
Doç. Dr. Uğuray Aydos Nükleer Tıp Anabilim Dalı
Prof. Dr. Yasemin Erten İç Hastalıkları Nefroloji Bilim Dalı
Prof. Dr. Yasemen Işık Mengü Çocuk Ve Ergen Ruh Sağlığı Anabilim Dalı
Prof. Dr. Yusuf Hakan Çavuşoğlu Çocuk Cerrahisi Anabilim Dalı
Prof. Dr. Yusuf Kemal Kemaloğlu Kulak Burun Boğaz Anabilim Dalı
Prof. Dr. Yusuf Kızıl Kulak Burun Boğaz Anabilim Dalı
Prof. Dr. Yusuf Ünal Anestezi Ve Reanimasyon Anabilim Dalı
Doç. Dr. Yasemin Taş Torun Çocuk Ve Ergen Ruh Sağlığı Anabilim Dalı
Doç. Dr. Yavuz Selim Kıyak Tıp Eğitimi Ve Bilişimi Anabilim Dalı
Doç Dr. Yeşim Yıldız Enfeksiyon Hastalıkları Anabilim Dalı
Dr. Öğr. Üyesi Yusuf Bahap Tıbbi Genetik Anabilimdalı
"""

titles = [
    "Prof. Dr.", "Prof Dr.", "Doç. Dr.", "Doç Dr.",
    "Dr. Öğr. Üyesi", "Dr. Öğr. Üye.", "Öğr. Gör. Dr.", "Ögr. Gör. Dr.",
    "Dr. Öğr. Gör.", "Prof."
]

def parse_line(line):
    line = line.strip()
    if not line: return None
    
    found_title = None
    for t in titles:
        if line.startswith(t):
            found_title = t
            break
            
    if not found_title:
        # Check if it's just a letter or something
        if len(line) < 5: return None
        return None
        
    content = line[len(found_title):].strip()
    
    # Split by spaces and look for the name/surname part
    # Usually the first 2 or 3 words are name and surname
    # The rest is the department
    
    parts = content.split()
    if len(parts) < 2: return None
    
    # Heuristic: name and surname are capitalized words at the start
    # Department often starts with "Genel", "İç", "Kardiyoloji", etc. or ends with "Anabilim Dalı"
    
    # Let's try to find "Anabilim", "Bilim", "Dal" to split the department
    dept_keywords = ["Anabilim", "Bilim", "Dal", "Hastalı", "Cerrahi", "Tıp", "Merkezi", "Birimi", "Laboratuvarı"]
    
    split_idx = -1
    for i in range(len(parts)):
        if parts[i] in ["Anabilimdalı", "Anabilim"]:
            # The previous 2 or 3 words are likely name and surname
            # But wait, it's better to find the LAST word of the name.
            # Usually names are 2 words (Name Surname) or 3 (Name Name Surname)
            # Let's assume the name is everything before the first word that starts the department
            pass

    # Better approach: The name and surname usually don't contain "Anabilim", "Bilim", etc.
    # And they are usually 2 or 3 words.
    
    # Common department starters
    dept_starters = [
        "Genel", "Kalp", "İç", "Kardiyoloji", "Acil", "Kadın", "Radyoloji", "Üroloji",
        "Deri", "Beyin", "Göz", "Göğüs", "Ortopedi", "Kulak", "Çocuk", "Halk", "Ruh",
        "Tıbbi", "Plastik", "İmmünoloji", "Fiziksel", "Adli", "Biyofizik", "Anatomi",
        "Fizyoloji", "Radyasyon", "Yenidoğan", "Anestezi", "Nükleer", "Histoloji",
        "Algoloji", "Tıp", "Nöroloji", "Enfeksiyon", "Geriatri", "Nefroloji", "Hematoloji",
        "Onkoloji", "Gastroenteroloji", "Romatoloji", "Endokrinoloji", "Patoloji", "Biyokimya",
        "Mikrobiyoloji", "Genetik", "Farmakoloji", "Embriyoloji", "Yoğun", "Cerrahi"
    ]
    
    name_parts = []
    dept_parts = []
    found_dept = False
    
    for i, word in enumerate(parts):
        if not found_dept:
            # If word is in dept_starters and it's not the first word of the name
            if word in dept_starters and i > 0:
                found_dept = True
                dept_parts.append(word)
            else:
                name_parts.append(word)
        else:
            dept_parts.append(word)
            
    if not found_dept:
        # Fallback: take last 2 words as dept if it's long? No.
        # Most names are 2 words.
        if len(parts) >= 3:
            name_parts = parts[:2]
            dept_parts = parts[2:]
        else:
            name_parts = parts
            dept_parts = ["Bilinmiyor"]

    name_str = " ".join(name_parts)
    # Split name_str into Ad and Soyad (last word is Soyad)
    name_split = name_str.rsplit(None, 1)
    ad = name_split[0] if len(name_split) > 1 else name_str
    soyad = name_split[1] if len(name_split) > 1 else ""
    
    return {
        "unvan": found_title,
        "ad": ad,
        "soyad": soyad,
        "bolum": " ".join(dept_parts)
    }

doctors = []
for line in raw_data.split('\n'):
    d = parse_line(line)
    if d: doctors.append(d)

unique_bolumler = sorted(list(set(d['bolum'] for d in doctors)))

# Map bolum name to id (starting from 100 to avoid conflicts with existing small IDs)
bolum_map = {name: i + 100 for i, name in enumerate(unique_bolumler)}

print("-- 1. BÖLÜMLER EKLEME SORGUSU")
print("INSERT INTO \"Bolumler\" (\"id\", \"ad\", \"isactive\") VALUES")
for name, bid in bolum_map.items():
    print(f"({bid}, '{name.replace(\"'\", \"''\")}', true),")
print(";")

print("\n-- 2. DOKTORLAR EKLEME SORGUSU")
print("INSERT INTO \"Doktorlar\" (\"unvan\", \"ad\", \"soyad\", \"bolumid\", \"uzmanlikalani\", \"isactive\") VALUES")
for d in doctors:
    bid = bolum_map.get(d['bolum'])
    print(f"('{d['unvan']}', '{d['ad'].replace(\"'\", \"''\")}', '{d['soyad'].replace(\"'\", \"''\")}', {bid}, '{d['bolum'].replace(\"'\", \"''\")}', true),")
print(";")
